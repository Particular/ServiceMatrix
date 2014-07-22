using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.Modeling;
using NServiceBusStudio.Automation.Commands;
using NServiceBusStudio.AutomationSetup;
using NuPattern;
using NuPattern.Library.Automation;
using NuPattern.Library.Commands;
using NuPattern.Library.ValueProviders;
using NuPattern.Runtime;
using NuPattern.Runtime.Bindings;
using NuPattern.VisualStudio.Solution;
using JsonConvert = NuPattern.Runtime.Serialization.JsonConvert;

namespace NServiceBusStudio
{
    public static class CustomAutomationExtensions
    {
        public static ICommandSettings CreateOrUpdateCommandSettings<T>(
            this IPatternElementSchema element,
            Guid id,
            string name,
            Expression<Func<T>> creationExpression)
            where T : Command
        {
            var commandSettings = element.DoCreateOrUpdateCommandSettings(id, name, creationExpression);

            if (!(typeof(T).IsAssignableTo(typeof(GenerateProductCodeCommand))))
            {
                return commandSettings;
            }

            var templateUriBinding = commandSettings.Properties.FirstOrDefault(p => p.Name == "TemplateUri");
            if (templateUriBinding == null || string.IsNullOrEmpty(templateUriBinding.Value))
            {
                return commandSettings;
            }

            var templateMappings = TemplateMappings.GetTuplesForBaseTemplate(templateUriBinding.Value);
            if (templateMappings.Length == 0 || (templateMappings.Length == 1 && templateMappings[0].Item2 == templateUriBinding.Value))
            {
                return commandSettings;
            }

            return element.DoCreateOrUpdateDispatcherCommandSettings(name, creationExpression, templateMappings);
        }

        public static ICommandSettings CreateOrUpdateDispatcherCommandSettings<T>(
            this IPatternElementSchema element,
            string name,
            Expression<Func<T>> partialCreationExpression,
            params Tuple<Guid, string, string>[] templatesPerVersion)
            where T : GenerateProductCodeCommand
        {
            return element.DoCreateOrUpdateDispatcherCommandSettings<T>(name, partialCreationExpression, templatesPerVersion);
        }

        public static ICommandSettings CreateOrUpdateAggregatorCommandSettings(
            this IPatternElementSchema element,
            string name,
            params ICommandSettings[] references)
        {
            var aggregatorCommandSettings = element.GetAutomationSettings<ICommandSettings>(name);
            if (aggregatorCommandSettings == null)
            {
                aggregatorCommandSettings = element.CreateAutomationSettings<ICommandSettings>(name);
            }

            aggregatorCommandSettings.TypeId = "NuPattern.Library.Commands.AggregatorCommand";
            aggregatorCommandSettings.Properties.Clear();
            aggregatorCommandSettings.Properties.Add(
                new PropertyBindingSettings
                {
                    Name = "CommandReferenceList",
                    Value =
                        JsonConvert.SerializeObject(
                            references.Select(c => new { CommandId = c.Id.ToString("D") }).ToArray(),
                            NuPattern.Runtime.Serialization.Formatting.Indented,
                            new NuPattern.Runtime.Serialization.JsonConverter[0])
                });

            return aggregatorCommandSettings;
        }

        private static ICommandSettings DoCreateOrUpdateCommandSettings<T>(
            this IPatternElementSchema element,
            Guid id,
            string name,
            Expression<Func<T>> creationExpression)
            where T : Command
        {
            return element.CreateOrUpdateAutomationSettings<ICommandSettings, T>(id, name, creationExpression);
        }

        private static ICommandSettings DoCreateOrUpdateDispatcherCommandSettings<T>(
            this IPatternElementSchema element,
            string name,
            Expression<Func<T>> partialCreationExpression,
            params Tuple<Guid, string, string>[] templatesPerVersion)
            where T : Command
        {
            return
                element.DoCreateOrUpdateDispatcherCommandSettings(
                    name,
                    templatesPerVersion.Select(t =>
                    {
                        var commandSetting = element.DoCreateOrUpdateCommandSettings(t.Item1, name + "_" + t.Item2, partialCreationExpression);
                        var existingProperty = commandSetting.Properties.FirstOrDefault(p => p.Name == "TemplateUri");
                        if (existingProperty != null)
                        {
                            existingProperty.Value = t.Item3;
                        }
                        else
                        {
                            commandSetting.Properties.Add(new PropertyBindingSettings { Name = "TemplateUri", Value = t.Item3 });
                        }

                        return Tuple.Create(t.Item2, commandSetting);
                    }).ToArray());
        }

        private static ICommandSettings DoCreateOrUpdateDispatcherCommandSettings(
            this IPatternElementSchema element,
            string name,
            params Tuple<string, ICommandSettings>[] references)
        {
            return ConfigureBindingSettings(
                element.GetAutomationSettings<ICommandSettings>(name) ?? element.CreateAutomationSettings<ICommandSettings>(name),
                () =>
                    new TargetNsbVersionDispatcherCommand
                    {
                        TargetNsbVersion =
                            BindingFor.ValueProvider<string>(new ElementPropertyValueProvider { PropertyName = "TargetNsbVersion" }),
                        CommandReferenceList =
                            BindingFor.Value<Collection<TargetNsbVersionPatternConditionalCommandReference>>(
                                JsonConvert.SerializeObject(
                                    references.Select(ct => new { CommandId = ct.Item2.Id, NsbVersionPattern = ct.Item1 }).ToArray(),
                                    NuPattern.Runtime.Serialization.Formatting.Indented,
                                    new NuPattern.Runtime.Serialization.JsonConverter[0]))
                    });
        }

        private static TSettings CreateOrUpdateAutomationSettings<TSettings, TImplementation>(
            this IPatternElementSchema element,
            Guid id,
            string name,
            Expression<Func<TImplementation>> creationExpression)
            where TSettings : class, IAutomationSettings, IBindingSettings
        {
            var settings = element.GetAutomationSettings<TSettings>(name) ?? element.CreateAutomationSettings<TSettings>(id, name);
            if (settings.Id != id)
            {
                throw new ArgumentException("Id doesn't match");
            }

            return ConfigureBindingSettings(settings, creationExpression);
        }

        private static TSettings CreateAutomationSettings<TSettings>(
            this IPatternElementSchema container,
            Guid id,
            string settingsName,
            Action<IAutomationSettingsSchema> initializer = null)
            where TSettings : class, IAutomationSettings
        {
            var automationSettings = default(TSettings);

            container.CreateAutomationSettingsSchema(aes =>
            {
                var aesMel = (ModelElement)aes;
                var classInfo = aesMel.Store.DomainDataDirectory.DomainClasses.First(c => typeof(TSettings).IsAssignableFrom(c.ImplementationClass));
                var extensionElement = (ExtensionElement)((ModelElement)aes).Partition.ElementFactory.CreateElement(classInfo, new[] { new PropertyAssignment(ElementFactory.IdPropertyAssignment, id) });
                ModelElement.AddExtension(aesMel, extensionElement);
                automationSettings = (TSettings)(object)extensionElement;

                aes.Name = settingsName;
                aes.AutomationType = classInfo.DisplayName;
                aes.Classification = automationSettings.Classification;

                if (initializer != null)
                    initializer(aes);
            });

            return automationSettings;
        }

        // lower level building blocks

        private static T ConfigureBindingSettings<T, U>(
            T settings,
            Expression<Func<U>> creationExpression)
            where T : IBindingSettings
        {
            var initExpression = creationExpression.Body as MemberInitExpression;
            if (initExpression == null || creationExpression.Parameters.Count != 0)
            {
                throw new ArgumentException("Expression is not a member init expression without parameters", "creationExpression");
            }

            return ConfigureBindingSettings(settings, initExpression);
        }

        private static T ConfigureBindingSettings<T>(
            T settings,
            MemberInitExpression initExpression)
            where T : IBindingSettings
        {
            settings.TypeId = GetTypeId(initExpression.Type);
            settings.Properties.Clear();
            settings.Properties.AddRange(initExpression.Bindings.Select(GetBindingSettings));

            return settings;
        }

        private static IPropertyBindingSettings GetBindingSettings(MemberBinding binding)
        {
            var assignment = binding as MemberAssignment;
            if (assignment == null)
            {
                throw new ArgumentException("Binding is not member assignment", "binding");
            }

            var memberName = assignment.Member.Name;
            var bindingMethodCall = assignment.Expression as MethodCallExpression;
            if (bindingMethodCall == null)
            {
                // simple value
                return CreateValueBindingSettings(assignment.Expression, memberName);
            }

            if (bindingMethodCall.Method.DeclaringType != typeof(BindingFor))
            {
                throw new ArgumentException("Binding value is a non-binding-conversion method call", "binding");
            }

            if (bindingMethodCall.Method.Name == "Value")
            {
                return CreateValueBindingSettings(bindingMethodCall.Arguments[0], memberName);
            }

            if (bindingMethodCall.Method.Name == "ValueProvider")
            {
                return CreateValueProviderBindingSettings(bindingMethodCall.Arguments[0], memberName);
            }

            throw new NotImplementedException();
        }

        static IPropertyBindingSettings CreateValueBindingSettings(Expression valueExpression, string memberName)
        {
            var constantExpression = valueExpression as ConstantExpression;

            var value = constantExpression != null ? constantExpression.Value : Expression.Lambda<Func<object>>(Expression.Convert(valueExpression, typeof(object))).Compile().Invoke();

            if (value is bool)
            {
                value = ((bool)value) ? "True" : "False";
            }

            return new PropertyBindingSettings
            {
                Name = memberName,
                Value = value.ToString()
            };
        }

        static IPropertyBindingSettings CreateValueProviderBindingSettings(Expression valueProviderExpression, string memberName)
        {
            if (valueProviderExpression.NodeType == ExpressionType.Convert)
            {
                valueProviderExpression = ((UnaryExpression)valueProviderExpression).Operand;
            }

            var initExpression = valueProviderExpression as MemberInitExpression;
            if (initExpression == null)
            {
                throw new ArgumentException("Expression for " + memberName + " is not a member init expression", "valueProviderExpression");
            }

            var valueProviderType = initExpression.Type;

            if (!valueProviderType.IsAssignableTo(typeof(IValueProvider)))
            {
                throw new ArgumentException("Expression for " + memberName + " is not value provider expression", "valueProviderExpression");
            }

            return new PropertyBindingSettings
            {
                Name = memberName,
                ValueProvider = ConfigureBindingSettings(new ValueProviderBindingSettings(), initExpression)
            };
        }

        private static string GetTypeId(Type type)
        {
            return type.FullName;
        }
    }

    public static class BindingFor
    {
        public static T Value<T>(string bindingValue)
        {
            return default(T);
        }

        public static T ValueProvider<T>(object valueProvider)
        {
            return default(T);
        }
    }
}
