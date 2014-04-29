using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NuPattern;
using NuPattern.Runtime;
using System.Collections.Generic;
using NuPattern.Diagnostics;

namespace NServiceBusStudio.Automation.Commands
{
    using Command = NuPattern.Runtime.Command;

    /// <summary>
	/// A custom command that performs some automation.
	/// </summary>
	[DisplayName("Execute Child Command Starting With")]
	[Category("Pattern Automation")]
	[Description("Executes a command on a child element.")]
	[CLSCompliant(false)]
    public class ChildCommandReference : Command
	{
		private static readonly ITracer tracer = Tracer.Get<ChildCommandReference>();

		public ChildCommandReference()
		{
			Recursive = false;
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

            if (CurrentElement.As<IApplication>().IsDirty &&
                CurrentElement.As<IApplication>().IsValidLicensed)
            {
                IEnumerable<IProductElement> children;

                if (Recursive)
                {
                    children = CurrentElement.GetChildren()
                        .Traverse(element => element.GetChildren().Concat((element is IProduct) ? new IProduct[] { } : element.As<IAbstractElement>().Extensions));
                }
                else
                {
                    children = CurrentElement.GetChildren();
                }

                var commands = children.SelectMany(c =>
                    c.AutomationExtensions.Where(a => a.Name.StartsWith(CommandNameStartsWidth)).OrderBy (a => a.Name));

                foreach (var command in commands)
                {
                    //System.Windows.Threading.Dispatcher.CurrentDispatcher.BeginInvoke(new Action(() => command.Execute()));
                    command.Execute();
                }
            }
		}
	}
}
