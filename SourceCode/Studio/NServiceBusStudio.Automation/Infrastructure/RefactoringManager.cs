using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using System.Diagnostics;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;

namespace NServiceBusStudio.Automation.Infrastructure
{
    [Export]
    public class RefactoringManager
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<RefactoringManager>();

        public ISolution Solution { get; set; }
        public IStatusBar StatusBar { get; set; }

        [ImportingConstructor]
        public RefactoringManager(ISolution solution, IStatusBar statusBar)
        {
            this.Solution = solution;
            this.StatusBar = statusBar;
        }

        public void RenameClass(string classNamespace, string classCurrentName, string classNewName)
        {
            SaveSolution();
            
            var ps = @".\Libs\NRefactory.RenameClass.exe";
            var args = String.Format("\"{0}\" {1} {2} {3}", this.Solution.PhysicalPath, classNamespace, classCurrentName, classNewName);

            var start = new ProcessStartInfo(ps, args)
            {
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
            };

            var process = Process.Start(start);

            process.EnableRaisingEvents = true;

            // Redirecting Output to VSStatusBar and Output Window
            process.BeginOutputReadLine();
            process.OutputDataReceived += (sender, e) => Log(LogType.Output, e.Data);

            // Redirecting Error to VSStatusBar and Output Window
            process.BeginErrorReadLine();
            process.ErrorDataReceived += (sender, e) => Log(LogType.Error, e.Data);

            process.WaitForExit();
        }

        private void SaveSolution()
        {
            this.Solution.As<EnvDTE.Solution>().SaveAs(this.Solution.Name);

            var projects = this.Solution.Find(x => x.Kind == ItemKind.Project);
            foreach (var project in projects)
            {
                project.As<EnvDTE.Project>().Save();
            }
        }

        private void Log(LogType logType, string data)
        {
            string logTypeDesc = "";

            if (String.IsNullOrEmpty(data))
                return;

            switch (logType)
            {
                case LogType.Error:
                    logTypeDesc = "Error: ";
                    break;
                case LogType.Output:
                    logTypeDesc = "";
                    break;
            }

            tracer.TraceInformation(logTypeDesc + data);
            this.StatusBar.DisplayMessage(logTypeDesc + data);
        }

    }

    public enum LogType
    {
        Output,
        Error
    }
}
