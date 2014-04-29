using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Composition;
using NuPattern.Runtime;
using Microsoft.VisualStudio.Shell;
using NuPattern.VisualStudio.Solution;

namespace NServiceBusStudio.Automation.Infrastructure.Authentication
{
    using Command = NuPattern.Runtime.Command;

    [DisplayName("Add authentication code for all the endpoints")]
    [Category("General")]
    [Description("Add authentication code for all the endpoints.")]
    [CLSCompliant(false)]
    public class AuthenticationAddedCommand : Command
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
