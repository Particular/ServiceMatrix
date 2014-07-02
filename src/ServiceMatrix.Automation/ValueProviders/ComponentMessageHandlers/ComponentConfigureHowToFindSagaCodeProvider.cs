using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NServiceBusStudio.Automation.ValueProviders.ComponentMessageHandlers
{
    using NServiceBusStudio.Automation.Model;
    using NuPattern.Runtime;

    [CLSCompliant(false)]
    [Category("General")]
    [Description("Generate ConfigureHowToFindSagaBody value and Set Code properties.")]
    [DisplayName("Returns ConfigureHowToFindSagaBody value")]
    public class ComponentConfigureHowToFindSagaCodeProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            return GenerateConfigureHowToFindSagaBody();
        }

        private string GenerateConfigureHowToFindSagaBody()
        {
            var targetNsbVersion = Component.As<IProductElement>().Root.As<IApplication>().TargetNsbVersion;
            //TODO: Generate appropriate mapping for NSB v4 vs NSB v5 .
            var sb = new StringBuilder();
            sb.AppendLine();

            var sagaStarters = new List<string>();
            sagaStarters.AddRange(Component.Subscribes.ProcessedCommandLinks.Where(x => x.StartsSaga).Select(x => x.CommandReference.Value.InstanceName));
            sagaStarters.AddRange(Component.Subscribes.SubscribedEventLinks.Where(x => x.StartsSaga).Select(x => x.EventReference.Value.InstanceName));
            sagaStarters.AddRange(Component.Subscribes.HandledMessageLinks.Where(x => x.StartsSaga).Select(x => x.MessageReference.Value.InstanceName));

            foreach (var cl in Component.Subscribes.ProcessedCommandLinks)
            {
                if (sagaStarters.Any(x => x != cl.CommandReference.Value.InstanceName))
                {
                    if (targetNsbVersion == TargetNsbVersion.Version4)
                    {
                        sb.AppendLine("            ConfigureMapping<" + cl.CommandReference.Value.InstanceName + ">(m => /* specify message property */).ToSaga(s =>  /* specify saga property */ );");
                    }
                    if (targetNsbVersion == TargetNsbVersion.Version5)
                    {
                        sb.AppendLine("            mapper.ConfigureMapping<" + cl.CommandReference.Value.InstanceName + ">(m => /* specify message property m.PropertyThatCorrelatesToSaga */).ToSaga(s => /* specify saga data property s.PropertyThatCorrelatesToMessage */);");
                    }
                }
            }

            foreach (var el in Component.Subscribes.SubscribedEventLinks)
            {
                if (sagaStarters.Any(x => x != el.EventReference.Value.InstanceName))
                {
                    if (targetNsbVersion == TargetNsbVersion.Version4)
                    {
                        sb.AppendLine("            ConfigureMapping<" + el.EventReference.Value.InstanceName + ">(m => /* specify message property */).ToSaga(s =>  /* specify saga property */ );");
                    }
                    if (targetNsbVersion == TargetNsbVersion.Version5)
                    {
                        sb.AppendLine("            mapper.ConfigureMapping<" + el.EventReference.Value.InstanceName + ">(m => /* specify message property m.PropertyThatCorrelatesToSaga */).ToSaga(s => /* specify saga data property s.PropertyThatCorrelatesToMessage */);");
                    }
                }
            }

            foreach (var hl in Component.Subscribes.HandledMessageLinks)
            {
                if (sagaStarters.Any(x => x != hl.MessageReference.Value.InstanceName))
                {
                    if (targetNsbVersion == TargetNsbVersion.Version4)
                    {
                        sb.AppendLine("            // ConfigureMapping<" + hl.MessageReference.Value.InstanceName + ">(m => /* specify message property */).ToSaga(s =>  /* specify saga property */ );");
                    }
                    if (targetNsbVersion == TargetNsbVersion.Version5)
                    {
                        sb.AppendLine("            // mapper.ConfigureMapping<" + hl.MessageReference.Value.InstanceName + ">(m => /* specify message property m.PropertyThatCorrelatesToSaga */).ToSaga(s => /* specify saga data property s.PropertyThatCorrelatesToMessage */);");
                    }
                }
            }

            return sb.ToString();
        }
    }
}
