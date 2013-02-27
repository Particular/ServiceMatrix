using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("On Application Loaded")]
    public class OnApplicationLoadedCommand : FeatureCommand
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }

        public override void Execute()
        {
            //this.Viewer.ShowContent(Guid.NewGuid(), "NServiceBus Details Pane", new NServiceBusStudio.Automation.CustomSolutionBuilder.Views.DetailsPanel());
            // this.CurrentElement.As<IApplication>().RaiseOnApplicationLoaded();
        }
    }
}
