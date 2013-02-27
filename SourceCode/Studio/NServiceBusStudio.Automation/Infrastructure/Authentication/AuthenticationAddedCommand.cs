using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using Microsoft.VisualStudio.Shell;

namespace NServiceBusStudio.Automation.Infrastructure.Authentication
{
    [DisplayName("Add authentication code for all the endpoints")]
    [Category("General")]
    [Description("Add authentication code for all the endpoints.")]
    [CLSCompliant(false)]
    public class AuthenticationAddedCommand : FeatureCommand
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

        [Required]
        [Import(AllowDefault = true)]
        public IPatternManager PatternManager
        {
            get;
            set;
        }

        [Required]
        [Import(AllowDefault = true)]
        public Microsoft.VisualStudio.TeamArchitect.PowerTools.ISolution Solution { get; set; }

        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            AuthenticationFeature.GenerateAuthenticationCodeOnEndpoints(this.CurrentElement.Root.As<IApplication>(), this.ServiceProvider);
        }
    }
}
