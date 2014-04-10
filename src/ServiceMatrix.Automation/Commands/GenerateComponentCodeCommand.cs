using NServiceBusStudio;
using NuPattern.Diagnostics;
using NuPattern.Library.Commands;
using ServiceMatrix.Diagramming.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NuPattern.Library.Commands
{
    public class GenerateComponentCodeCommand : GenerateProductCodeCommandCustom
    {
        private static readonly ITracer tracer = Tracer.Get<GenerateComponentCodeCommand>();

        public bool CheckIsDeployed { get; set; }

        public bool CheckIsNotUnfoldedCustomCode { get; set; }

        public override void Execute()
        {
            var app = this.CurrentElement.Root.As<IApplication>();
            var component = this.CurrentElement.As<IComponent>();

            if (CheckIsDeployed)
            {
                var isDeployed = app.Design.Endpoints.GetAll()
                    .Any(ep => ep.EndpointComponents.AbstractComponentLinks.Any(cl => cl.ComponentReference.Value == component));

                if (!isDeployed)
                {
                    tracer.Info("Component not deployed. Not unfolding T4 file.");
                    return;
                }
            }

            if (CheckIsNotUnfoldedCustomCode)
            {
                if (component.UnfoldedCustomCode)
                {
                    tracer.Info("Component custom code already unfolded. Not unfolding T4 file.");
                    return;
                }
            }

            base.Execute();
        }
    }
}
