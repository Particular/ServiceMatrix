using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using AbstractEndpoint;
using Microsoft.VisualStudio.Shell;

namespace NServiceBusStudio.Automation.Infrastructure.Authentication
{
    [DisplayName("Add Authentication Elements")]
    [Category("General")]
    [Description("Generates an Authentication element on Infrastructure-Security.")]
    [CLSCompliant(false)]
    public class AddAuthenticationCommand : FeatureCommand
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
        public Microsoft.VisualStudio.TeamArchitect.PowerTools.ISolution Solution { get; set; }


        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            var infrastructure = this.CurrentElement.As<IInfrastructure>();
            var security = infrastructure.Security?? infrastructure.CreateSecurity("Security");
            var authentication = security.CreateAuthentication("Authentication");
            AuthenticationFeature.InitializeAuthenticationValues(this.CurrentElement.Root.As<IApplication>(), Solution, this.ServiceProvider);
        }
    }

    [DisplayName("Checks if Authentication Can Be Created")]
    [Category("General")]
    [Description("Checks if Authentication Can Be Created")]
    [CLSCompliant(false)]
    public class CanCreateAuthenticationCondition : Condition
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

        public override bool  Evaluate()
        {
 	        return (this.CurrentElement.As<IInfrastructure>().Security == null
                    || this.CurrentElement.As<IInfrastructure>().Security.Authentication == null);

        }
    }
}
