using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using Microsoft.VisualStudio.Shell;
using NuPattern.Runtime;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.Infrastructure.Authentication
{
    [DisplayName("Add authentication code for all the endpoints")]
    [Category("General")]
    [Description("Add authentication code for all the endpoints.")]
    [CLSCompliant(false)]
    public class AuthenticationAddedCommand : NuPattern.Runtime.Command
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
        public ISolution Solution { get; set; }

        [Import(typeof(SVsServiceProvider))]
        public IServiceProvider ServiceProvider { get; set; }

        public override void Execute()
        {
            AuthenticationFeature.GenerateAuthenticationCodeOnEndpoints(CurrentElement.Root.As<IApplication>(), ServiceProvider);
        }
    }
}
