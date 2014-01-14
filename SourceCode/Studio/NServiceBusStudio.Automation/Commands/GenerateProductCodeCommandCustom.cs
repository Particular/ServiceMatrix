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
    public class GenerateProductCodeCommandCustom : GenerateProductCodeCommand
    {
        private static readonly ITracer tracer = Tracer.Get<GenerateProductCodeCommandCustom>();

        public override void Execute()
        {
            try
            {
                base.Execute();
            }
            catch (IOException ex) 
            {
                tracer.Error(ex, String.Format("The file {0} is locked by a process and cannot be regenerated. Please, close the locking process and try again.", this.TargetFileName));
            }
        }
    }
}
