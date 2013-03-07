using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TeamArchitect.PowerTools;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuPattern.Runtime;
using System.IO;
using System.Net.Http;
using System.Diagnostics;
using Microsoft.Win32;
using System.Timers;
using System.Net;
using System.Collections.Specialized;

namespace NServiceBusStudio.Automation.Infrastructure
{
    [Export]
    public class StatisticsManager
    {
        // Statistics generation constants
        public const string StatisticsListenerNamespace = "NServiceBusStudio";
        public const string StatisticsFileExtension = "logging";
        public const string TextWriterListenerName = "SolutionTextWriterTraceListener";

        // Statistics upload constants
        public const string StatisticsRegistryKey = @"SOFTWARE\NServiceBus";
        public const string UsageDataCollectionApprovedValueName = @"UsageDataCollectionApproved";
        public const string LastUploadValueName = @"LastUpload";
        public const string UploadEndpointURLValueName = @"UploadEndpointURL";
        public static TimeSpan UploadStatisticsInterval = new TimeSpan(7, 0, 0, 0);

        public TextWriterTraceListener TextWriterListener { get; set; }
        public ISolution Solution { get; set; }
        public Timer TimerUploadStatistics { get; set; }

        public string SolutionLoggingFile 
        {
            get
            {
                if (this.Solution != null && this.Solution.IsOpen)
                {
                    return Path.ChangeExtension(this.Solution.PhysicalPath, StatisticsManager.StatisticsFileExtension);
                }

                return null;
            }
        }

        public string SolutionLastUploadFile
        {
            get { return this.SolutionLoggingFile + ".lastupload"; }
        }

        // Registry Getters/Setters
        public RegistryKey StatisticsKey
        {
            get { return RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(StatisticsManager.StatisticsRegistryKey); }
        }

        public bool ShouldUpload 
        {
            get { return (string)this.StatisticsKey.GetValue(StatisticsManager.UsageDataCollectionApprovedValueName, null) == "1"; }
        }

        public DateTime? LastUpload
        {
            get { return File.Exists(this.SolutionLastUploadFile) ? (DateTime?) DateTime.Parse(File.ReadAllText(this.SolutionLastUploadFile)) : null; }
            set { File.WriteAllText (this.SolutionLastUploadFile, value.ToString()); }
        }

        public string UploadEndpointURL
        {
            get { return (string)this.StatisticsKey.GetValue(StatisticsManager.UploadEndpointURLValueName, null); }
        }

        [ImportingConstructor]
        public StatisticsManager([Import] ISolution solution, [Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            this.Solution = solution;
            StartListening(serviceProvider);
        }

        private void StartListening(IServiceProvider serviceProvider)
        {
            var events = serviceProvider.TryGetService<ISolutionEvents>();

            events.SolutionOpened += (s, e) =>
            {
                InitializeStatisticsCollection();
            };

            events.SolutionClosed += (s, e) =>
            {
                StopStatisticsCollection();
            };

            if (this.Solution.IsOpen)
            {
                InitializeStatisticsCollection();
            }

            this.ConfigureStatisticsUpload();
        }

        private void ConfigureStatisticsUpload()
        {
            // Configure Statistics send
            if (!this.ShouldUpload)
            {
                return;
            }

            TimeSpan nextUpload = default(TimeSpan);
            if (!this.LastUpload.HasValue) 
            {
                // New Solution - Upload in UploadStatisticsInterval
                this.LastUpload = DateTime.Now;
                nextUpload = StatisticsManager.UploadStatisticsInterval; 
            }
            else if (this.LastUpload.Value.Add(StatisticsManager.UploadStatisticsInterval) < DateTime.Now)
            {
                // Upload in 10 minutes - To avoid do it now!
                nextUpload = new TimeSpan(0, 10, 0);
            }
            else
            {
                // Upload when LastUpload + UploadStatisticsInterval is achieved
                nextUpload = LastUpload.Value.Add(StatisticsManager.UploadStatisticsInterval) - DateTime.Now;
            }

            TimerUploadStatistics = new Timer();
            TimerUploadStatistics.Interval = nextUpload.TotalMilliseconds;
            TimerUploadStatistics.Start();
            TimerUploadStatistics.Elapsed += (s, e) => this.UploadStatistics();
        }

        private void InitializeStatisticsCollection()
        {
            this.TextWriterListener = new TextWriterTraceListener(this.SolutionLoggingFile, StatisticsManager.TextWriterListenerName);
            Tracer.AddListener(StatisticsManager.StatisticsListenerNamespace, this.TextWriterListener);
        }

        private void StopStatisticsCollection()
        {
            this.TextWriterListener.Flush();
            this.TextWriterListener.Dispose();
            this.TextWriterListener = null;
            Tracer.RemoveListener(StatisticsManager.StatisticsListenerNamespace, StatisticsManager.TextWriterListenerName);
        }

        private void UploadStatistics()
        {
            if (this.SolutionLoggingFile == null || !File.Exists (this.SolutionLoggingFile) || String.IsNullOrEmpty (this.UploadEndpointURL))
            {
                return;
            }

            this.StopStatisticsCollection();

            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    var resp = wc.UploadFile(this.UploadEndpointURL, "POST", this.SolutionLoggingFile);

                    if (Encoding.ASCII.GetString(resp) != "\"ok\"")
                        return;
                }

                File.Delete(this.SolutionLoggingFile);
                TimerUploadStatistics.Stop();
                this.LastUpload = DateTime.Now;
            }
            finally
            {
                this.InitializeStatisticsCollection();
            }
        }

        
    }
}
