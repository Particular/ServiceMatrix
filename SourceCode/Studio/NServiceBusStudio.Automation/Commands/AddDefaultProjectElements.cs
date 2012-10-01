using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Patterning.Runtime;
using System.ComponentModel;

namespace NServiceBusStudio.Automation.Commands
{
    [DisplayName("Create Default Project Elements")]
    [Category("General")]
    [Description("Creates Contract and InternalMessages project elements.")]
    [CLSCompliant(false)]
    public class AddDefaultProjectElements : FeatureCommand
    {
        /// <summary>
        /// Gets or sets the current element.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement
        {
            get;
            set;
        }


        public override void Execute()
        {
            //var app = this.CurrentElement.As<IApplication>();
            //app.Design.CreateContractsProject(string.Format("{0}.Contract", app.InstanceName));
            //app.Design.CreateInternalMessagesProject(string.Format("{0}.InternalMessages", app.InstanceName));
        }
    }
}
