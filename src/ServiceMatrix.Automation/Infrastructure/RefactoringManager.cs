namespace NServiceBusStudio.Automation.Infrastructure
{
    using System;
    using System.ComponentModel.Composition;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using NuPattern.Runtime;
    using Microsoft.VisualStudio.Shell;
    using NuPattern.Diagnostics;
    using NuPattern.VisualStudio.Solution;
    using NuPattern.VisualStudio;
    using NuPattern;
    using System.Collections.Generic;
    using System.Windows.Threading;
    using EnvDTE;
    using Process = System.Diagnostics.Process;

    [Export]
    public class RefactoringManager
    {
        private static readonly ITracer tracer = Tracer.Get<RefactoringManager>();
        private FileSystemWatcher Watcher;

        public ISolution Solution { get; set; }

        public Dispatcher Dispatcher { get; set; }
        
        [Import]
        public IStatusBar StatusBar { get; set; }

        [Import]
        public IPatternManager PatternManager { get; set; }

        [Import]
        public IUriReferenceService UriService { get; set; }

        [ImportingConstructor]
        public RefactoringManager([Import] ISolution solution, [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            Solution = solution;
            Dispatcher = Dispatcher.CurrentDispatcher;
            StartListening(serviceProvider);
        }

        private void StartListening(IServiceProvider serviceProvider)
        {
            var events = serviceProvider.TryGetService<ISolutionEvents>();

            events.SolutionOpened += (s, e) => {
                InitializeWatcher();
            };
            
            events.SolutionClosed += (s, e) => {
                if (Watcher != null)
                {
                    Watcher.EnableRaisingEvents = false;
                    Watcher = null;
                }
            };

            if (Solution.IsOpen)
            {
                InitializeWatcher();
            }
        }

        private void InitializeWatcher()
        {
            Watcher = new FileSystemWatcher(Path.GetDirectoryName(Solution.PhysicalPath), "*.cs")
            {
                IncludeSubdirectories = true
            };

            Watcher.Renamed += new RenamedEventHandler(Watcher_Renamed);
            Watcher.EnableRaisingEvents = true;
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs renamedFile)
        {
            // Get Reference Uri for renamed File
            var item = Solution.Find(renamedFile.Name).FirstOrDefault();
            if (item == null)
            {
                return;
            }

            var referenceUri = UriService.CreateUri(item);

            // Get Root Pattern Elements 
            var rootElements = PatternManager.Products.SelectMany(x => x.Views.SelectMany(v => v.AllElements));
            // Get all Pattern Elements 
            var allElements = rootElements.Traverse<IProductElement>(e => e.GetChildren()); 

            // Get related elements to the Renamed File
            var relatedElements = allElements.Where(e => e.References.Any(r => r.Value == referenceUri.ToString()) &&
                                                         e.InstanceName == Path.GetFileNameWithoutExtension(renamedFile.OldName))
                                             .ToList();
            
            Dispatcher.BeginInvoke(new Action(() =>
                // Rename related elements with new name
                relatedElements.ForEach(x => x.InstanceName = Path.GetFileNameWithoutExtension(renamedFile.Name))
            ));
        }

        public void RenameClass(string classNamespace, string classCurrentName, string classNewName)
        {
            SaveSolution();

            var currentDirectory = Path.GetDirectoryName(GetType().Assembly.Location);
            var ps = Path.Combine (currentDirectory, @"Libs\NRefactory.RenameClass.exe");
            
            var args = String.Format("\"{0}\" {1} {2} {3}", Solution.PhysicalPath, classNamespace, classCurrentName, classNewName);

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

            Log(LogType.Output, "Ready");
        }

        public void RenameNamespaces(string currentName, string newName, IDictionary<string, string> currentNewNamespace)
        {
            SaveSolution();

            Log(LogType.Output, "Loading solution...");
            Log(LogType.Output, "Finding references...");

            var projects = Solution.Find(x => x.Kind == ItemKind.Project);
            foreach (var project in projects)
            {
                foreach (var item in project.Items)
                {
                    RenameNamespacesRecursive(item, currentName, newName, currentNewNamespace);
                }
            }

            Log(LogType.Output, "Ready");
        }

        private void RenameNamespacesRecursive(IItemContainer itemContainer, string currentName, string newName, IDictionary<string, string> currentNewNamespace)
        {
            if (itemContainer.Kind == ItemKind.Folder)
            {
                // Rename namespace on child files
                foreach (var item in itemContainer.Items)
                {
                    RenameNamespacesRecursive(item, currentName, newName, currentNewNamespace);
                }

                // Do folder renaming
                if (itemContainer.Name == currentName)
                {
                    itemContainer.Rename(newName);
                }
            }
            else
            {
                // File exists?
                if (!File.Exists(itemContainer.PhysicalPath))
                {
                    return;
                }

                // Do rename
                var fileText = File.ReadAllText(itemContainer.PhysicalPath);
                foreach (var renameNamespace in currentNewNamespace)
                {
                    fileText = fileText.Replace(renameNamespace.Key, renameNamespace.Value);
                }
                File.WriteAllText(itemContainer.PhysicalPath, fileText);
            }
        }

        private void SaveSolution()
        {
            Solution.As<Solution>().SaveAs(Solution.PhysicalPath);

            var projects = Solution.Find(x => x.Kind == ItemKind.Project);
            foreach (var project in projects)
            {
                project.As<Project>().Save();
            }
        }

        private void Log(LogType logType, string data)
        {
            var logTypeDesc = "";

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

            tracer.Info(logTypeDesc + data);
            StatusBar.DisplayMessage(logTypeDesc + data);
        }


        
    }

    public enum LogType
    {
        Output,
        Error
    }
}
