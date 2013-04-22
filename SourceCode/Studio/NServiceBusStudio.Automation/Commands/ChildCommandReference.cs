using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NuPattern;
using NuPattern.Extensibility;
using NuPattern.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using System.Collections.Generic;

namespace NServiceBusStudio.Automation.Commands
{
	/// <summary>
	/// A custom command that performs some automation.
	/// </summary>
	[DisplayName("Execute Child Command Starting With")]
	[Category("Pattern Automation")]
	[Description("Executes a command on a child element.")]
	[CLSCompliant(false)]
	public class ChildCommandReference : FeatureCommand
	{
		private static readonly ITraceSource tracer = Tracer.GetSourceFor<ChildCommandReference>();

		public ChildCommandReference()
		{
			this.Recursive = false;
		}

		/// <summary>
		/// Gets or sets whether to locate the command recursively 
		/// in all descendents.
		/// </summary>
		[DefaultValue(false)]
		[Required(AllowEmptyStrings = false)]
		[DisplayName("Recursive")]
		[Description("Whether to locate the command recursively in all descendants.")]
		public bool Recursive { get; set; }

		/// <summary>
		/// Gets or sets the name of the command to execute.
		/// </summary>
		[Required(AllowEmptyStrings = false)]
		[DisplayName("Command Name Starts With")]
		[Description("Matching the starts of the command name defined in the element.")]
		public string CommandNameStartsWidth { get; set; }

		/// <summary>
		/// Gets or sets the current element.
		/// </summary>
		[Required]
		[Import(AllowDefault = true)]
		public IProductElement CurrentElement { get; set; }

		/// <summary>
		/// Executes the referenced commmand or commands.
		/// </summary>
		public override void Execute()
		{
			Validator.ValidateObject(this, new ValidationContext(this, null, null), true);

            if (this.CurrentElement.As<IApplication>().IsDirty &&
                this.CurrentElement.As<IApplication>().IsValidLicensed)
            {
                var children = default(IEnumerable<IProductElement>);

                if (this.Recursive)
                {
                    children = this.CurrentElement.GetChildren()
                        .Traverse(element => element.GetChildren().Concat((element is IProduct) ? new IProduct[] { } : element.As<IAbstractElement>().Extensions));
                }
                else
                {
                    children = this.CurrentElement.GetChildren();
                }

                var commands = children.SelectMany(c =>
                    c.AutomationExtensions.Where(a => a.Name.StartsWith(this.CommandNameStartsWidth)).OrderBy (a => a.Name));

                foreach (var command in commands)
                {
                    //System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => command.Execute()));
                    command.Execute();
                }
            }
		}
	}
}
