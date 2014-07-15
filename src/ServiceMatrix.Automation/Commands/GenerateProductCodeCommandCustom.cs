using System;
using System.IO;
using NuPattern.Diagnostics;

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
