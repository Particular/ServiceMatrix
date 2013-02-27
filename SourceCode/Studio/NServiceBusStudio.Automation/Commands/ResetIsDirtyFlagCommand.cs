using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuPattern.Runtime;

namespace NServiceBusStudio.Automation.Commands
{
    public class ResetIsDirtyFlagCommand : FeatureCommand
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
            this.CurrentElement.As<IApplication>().IsDirty = false;
        }
    }
}
