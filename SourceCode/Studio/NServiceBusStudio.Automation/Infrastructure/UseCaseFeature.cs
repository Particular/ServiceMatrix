using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;
using AbstractEndpoint;
using NuPattern;
using NuPattern.Runtime.UI;
using NuPattern.VisualStudio.Solution;
using NuPattern.VisualStudio;
using NuPattern.Presentation;
using NuPattern.Runtime.UI.Views;
using NuPattern.Runtime.UI.ViewModels;

namespace NServiceBusStudio.Automation.Infrastructure
{
    public class UseCaseFeature :  IInfrastructureFeature
    {
        private IPatternManager PatternManager { get; set; }

        // Initialization
        public void Initialize(IApplication app, IProductElement infrastructure, IServiceProvider serviceProvider, IPatternManager patternManager)
        {
            this.PatternManager = patternManager;
            UpdateElementsForUseCase(app, serviceProvider.TryGetService<ISolution>(), serviceProvider);

            var handler = new EventHandler((s, e) =>
            {
                UpdateElementsForUseCase(app, serviceProvider.TryGetService<ISolution>(), serviceProvider);
            });

            // Listen for a new endpoint initialization
            // in order to add the required artifacts (typically menu items)
            app.OnInitializingEndpoint += handler;
            Command.ElementInitialized += handler;
            Event.ElementInitialized += handler;
            Component.ElementInitialized += handler;
        }

        public void UpdateElementsForUseCase(IApplication app, ISolution solution, IServiceProvider sp)
        {
            //app.SetEndpointsMenuItems("Start Use Case"
            //    , (e, a) => StartUseCase(e, a, sp)
            //    , (e, a) => true);
        }

        private void StartUseCase(IAbstractEndpoint e, IApplication a, IServiceProvider sp)
        {
            var viewModel = new AddNewNodeViewModel(
                a.Design.UseCases.UseCase.Select(uc => uc.As<IProductElement>())
                , a.Design.UseCases.As<IProductElement>().EnsureChildContainer("UseCase")
                , sp.TryGetService<IUserMessageService>());

            var view = new AddNewNodeView();
            view.DataContext = viewModel;
            view.Title = String.Format("Start new Use Case from {0}", e.As<IProductElement>().InstanceName);
            view.Owner = System.Windows.Application.Current.MainWindow;
            view.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            using (new MouseCursor(System.Windows.Input.Cursors.Arrow))
            {
                if (view.ShowDialog().GetValueOrDefault())
                {
                    using (new MouseCursor(System.Windows.Input.Cursors.Wait))
                    {
                        var useCase = a.Design.UseCases.CreateUseCase(viewModel.InstanceName);
                        useCase.AddEndpointStartingUseCase(e);
                        //useCase.AddRelatedElement(e.As<IProductElement>());
                    }
                }
            }
        }

        public void AddEndpointToUseCase(IAbstractEndpoint endpoint, IApplication application, IServiceProvider sp)
        {
            var command = new Commands.AddReferenceToUseCaseCommand { CurrentElement = endpoint.As<IProductElement>()};
            command.Execute();
        }
    }
}
