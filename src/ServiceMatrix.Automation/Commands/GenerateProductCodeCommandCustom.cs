using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell.Interop;
using NuPattern.Diagnostics;
using NuPattern.VisualStudio;

namespace NuPattern.Library.Commands
{
    public class GenerateProductCodeCommandCustom : GenerateProductCodeCommand
    {
        private static readonly ITracer tracer = Tracer.Get<GenerateProductCodeCommandCustom>();

        private DTE dte;

        [Browsable(false)]
        internal DTE Dte
        {
            get
            {
                return dte ?? (dte = ServiceProvider.GetService<SDTE, DTE>());
            }
        }

        [Required]
        [Import(AllowDefault = true)]
        public IUserMessageService UserMessageService { get; set; }

        public override void Execute()
        {
            EnsureNoProjectDesignersAreOpen();

            try
            {
                base.Execute();
            }
            catch (IOException ex) 
            {
                tracer.Error(ex, String.Format("The file {0} is locked by a process and cannot be regenerated. Please, close the locking process and try again.", this.TargetFileName));
            }
        }

        void EnsureNoProjectDesignersAreOpen()
        {
            var notified = UserMessageService == null;

            // TODO: Remove this function when NuPattern implements a workaround. 
            // The workaround is to close all the project designers, which can cause exceptions in NuPattern's t4 unfold logic.
            // Related NuPattern issue: https://github.com/NuPattern/NuPattern/issues/2
            foreach (var window in Dte.Windows.OfType<Window>().Where(w => w.Type == vsWindowType.vsWindowTypeDocument))
            {
                try
                {
                    // just so this expression compiles
                    var ignore = window.ProjectItem;
                }
                catch (InvalidCastException)
                {
                    if (!notified)
                    {
                        notified = true;

                        UserMessageService.ShowInformation("ServiceMatrix has detected that some Project Designers are open. These designers will now be closed in order to proceed with code generation.");
                    }

                    window.Close();
                }
            }
        }
    }
}
