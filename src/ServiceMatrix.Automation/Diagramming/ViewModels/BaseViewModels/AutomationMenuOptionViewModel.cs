using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Input;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using NServiceBusStudio.Automation.Properties;
using NuPattern;
using NuPattern.Diagnostics;
using NuPattern.Presentation;
using NuPattern.Reflection;
using NuPattern.Runtime;
using NuPattern.Runtime.Automation;
using NuPattern.Runtime.UI;
using NuPattern.Runtime.UI.ViewModels;
using NuPattern.VisualStudio;

namespace ServiceMatrix.Diagramming.ViewModels.BaseViewModels
{
    /// <summary>
    /// The view model for an automation menu.
    /// </summary>
    internal class AutomationMenuOptionViewModel : MenuOptionViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationMenuOptionViewModel"/> class.
        /// </summary>
        /// <param name="automation">The menu execution behavior.</param>
        public AutomationMenuOptionViewModel(IAutomationExtension automation)
            : this(automation, GetCaption(automation), GetImagePath(automation), GetSortOrder(automation))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AutomationMenuOptionViewModel"/> class.
        /// </summary>
        /// <param name="automation">The menu execution behavior.</param>
        /// <param name="caption">The caption.</param>
        /// <param name="imagePath">The path to the icon.</param>
        /// <param name="sortOrder">The sort order for the menu entry.</param>
        public AutomationMenuOptionViewModel(IAutomationExtension automation, string caption, string imagePath, long sortOrder)
            : base(caption)
        {
            Command = new AutomationCommand(this, automation);
            ImagePath = imagePath;
            IconType = string.IsNullOrEmpty(ImagePath) ? IconType.None : IconType.Image;
            SortOrder = sortOrder;
        }

        internal void ReEvaluateCommand()
        {
            ((AutomationCommand)Command).OnCanExecuteChanged();
        }

        private static string GetCaption(IAutomationExtension automation)
        {
            Guard.NotNull(() => automation, automation);

            var menu = automation as IMenuCommand;
            return menu != null ? menu.Text : null;
        }

        private static string GetImagePath(IAutomationExtension automation)
        {
            Guard.NotNull(() => automation, automation);

            var menu = automation as IAutomationMenuCommand;
            return menu != null ? menu.IconPath : null;
        }

        private static long GetSortOrder(IAutomationExtension automation)
        {
            Guard.NotNull(() => automation, automation);

            var menu = automation as IAutomationMenuCommand;
            return menu != null ? menu.SortOrder : 0;
        }

        private class NullQueryStatus : ICommandStatus
        {
            public void QueryStatus(IMenuCommand menu)
            {
            }
        }

        private class AutomationCommand : System.Windows.Input.ICommand
        {
            private static readonly Dictionary<string, Action<MenuOptionViewModel, IMenuCommand>> propertyMappings =
                new Dictionary<string, Action<MenuOptionViewModel, IMenuCommand>>
                {
                    { Reflector<IMenuCommand>.GetProperty(x => x.Visible).Name, (vm, m) => vm.IsVisible = m.Visible },
                    { Reflector<IMenuCommand>.GetProperty(x => x.Enabled).Name, (vm, m) => vm.IsEnabled = m.Enabled }
                };

            private static ITracer tracer = Tracer.Get<AutomationCommand>();

            private MenuOptionViewModel parent;
            private IAutomationExtension automation;
            private ICommandStatus status;
            private IMenuCommand menu;

            public AutomationCommand(MenuOptionViewModel parent, IAutomationExtension automation)
            {
                this.parent = parent;
                this.automation = automation;
                menu = automation as IMenuCommand;
                status = automation as ICommandStatus ?? new NullQueryStatus();

                parent.IsVisible = menu == null || menu.Visible;
                parent.IsEnabled = menu == null || menu.Enabled;

                var propertyChanged = automation as INotifyPropertyChanged;

                if (propertyChanged != null)
                {
                    propertyChanged.PropertyChanged += OnMenuPropertyChanged;
                }
            }

            public event EventHandler CanExecuteChanged;

            [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
            public bool CanExecute(object parameter)
            {
                if (menu == null)
                {
                    return true;
                }

                var propertyChanged = automation as INotifyPropertyChanged;

                try
                {
                    // Prevent re-entrancy on query status
                    if (propertyChanged != null)
                        propertyChanged.PropertyChanged -= OnMenuPropertyChanged;

                    status.QueryStatus(menu);
                    parent.IsEnabled = menu.Enabled;
                    parent.IsVisible = menu.Visible;
                }
                catch (Exception e)
                {
                    tracer.Error(e, Resources.AutomationCommand_QueryStatusFailed);
                    return false;
                }
                finally
                {
                    // Enable status monitoring again.
                    if (propertyChanged != null)
                        propertyChanged.PropertyChanged += OnMenuPropertyChanged;
                }

                return menu.Enabled;
            }

            public void Execute(object parameter)
            {
                tracer.ShieldUI(
                    () =>
                    {
                        using (new MouseCursor(Cursors.Wait))
                        {
                            automation.Execute();
                        }
                    },
                    Resources.AutomationCommand_CommandExecuteFailed,
                    automation.Name);
            }

            internal void OnCanExecuteChanged()
            {
                if (CanExecuteChanged != null)
                {
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }

            private void OnMenuPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                Action<MenuOptionViewModel, IMenuCommand> propertyName;
                if (propertyMappings.TryGetValue(e.PropertyName, out propertyName))
                {
                    propertyName(parent, menu);
                    OnCanExecuteChanged();
                }
            }
        }
    }
}