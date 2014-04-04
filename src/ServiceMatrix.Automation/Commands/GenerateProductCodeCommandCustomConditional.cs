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
    public class GenerateProductCodeCommandCustomConditional : GenerateProductCodeCommandCustom
    {
        private static readonly ITracer tracer = Tracer.Get<GenerateProductCodeCommandCustomConditional>();

        public string PropertyName { get; set; }

        public string Value { get; set; }

        public override void Execute()
        {
            var property = this.CurrentElement.Properties.FirstOrDefault (x => x.DefinitionName == this.PropertyName);

            if (property == null)
            {
                tracer.Error ("Property '{0}' not found on current element.", this.PropertyName);
                return;
            }

            if (String.Equals(property.RawValue.ToString(), this.Value, StringComparison.InvariantCultureIgnoreCase))
            {
                base.Execute();
            }
            else
            {
                tracer.Error ("Property '{0}' not match expected value: {1}. Current value: {2}.", this.PropertyName, this.Value, property.RawValue.ToString());
            }
        }
    }
}
