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
using NServiceBusStudio.Automation.Extensions;
using Microsoft.VisualStudio.ExtensionManager;

namespace NServiceBusStudio.Automation.Infrastructure
{
    [Export]
    public class StatisticsManager
    {
        private static readonly ITraceSource tracer = Tracer.GetSourceFor<StatisticsManager>();

        // Statistics generation constants
        public const string StatisticsListenerNamespace = "NServiceBusStudio";
        public const string StatisticsFileExtension = "logging";
        public const string TextWriterListenerName = "SolutionTextWriterTraceListener";

        // Statistics upload constants
        public const string StatisticsRegistryKey = @"SOFTWARE\Particular\ServiceBus";
        public const string UsageDataCollectionApprovedValueName = @"UsageDataCollectionApproved";
        public const string LastUploadValueName = @"LastUpload";
        public const string UploadEndpointURLValueName = @"UploadEndpointURL";
        public static TimeSpan UploadStatisticsInterval = new TimeSpan(7, 0, 0, 0); // 1 week
        public static TimeSpan CleanStatisticsInterval = new TimeSpan(120, 0, 0, 0); // 4 months

        public TextWriterTraceListener TextWriterListener { get; set; }
        public Timer TimerUploadStatistics { get; set; }

        public string LoggingFile
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(this.GetType().Assembly.Location), String.Format("{0}-{1}.{2}", Environment.MachineName, Environment.UserName, StatisticsManager.StatisticsFileExtension));
            }
        }

        public string LastUploadFile
        {
            get { return this.LoggingFile + ".lastupload"; }
        }

        // Registry Getters/Setters
        public RegistryKey StatisticsKey
        {
            get { return RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(StatisticsManager.StatisticsRegistryKey); }
        }

        public bool ShouldUpload
        {
            get { return this.StatisticsKey != null && (string)this.StatisticsKey.GetValue(StatisticsManager.UsageDataCollectionApprovedValueName, null) == "1"; }
        }

        public DateTime? LastUpload
        {
            get { return File.Exists(this.LastUploadFile) ? (DateTime?)DateTime.Parse(File.ReadAllText(this.LastUploadFile)) : null; }
            set { File.WriteAllText(this.LastUploadFile, value.ToString()); }
        }

        public string UploadEndpointURL
        {
            get { return (string)this.StatisticsKey.GetValue(StatisticsManager.UploadEndpointURLValueName, null); }
        }

        [ImportingConstructor]
        public StatisticsManager([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
            this.StartListening();
        }

        private void StartListening()
        {
            this.StartCollectingStatistics();
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
            else if (this.LastUpload.Value.Add(StatisticsManager.CleanStatisticsInterval) < DateTime.Now)
            {
                // Clean statistics
                this.StopCollectingStatistics();
                File.Delete(this.LoggingFile);
                this.StartCollectingStatistics();

                // Upload in UploadStatisticsInterval
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

        public void StartCollectingStatistics()
        {
            if (this.TextWriterListener == null && this.ShouldUpload)
            {
                var shouldInitializeFile = !File.Exists(this.LoggingFile);

                this.TextWriterListener = new StatisticsTextWriterTraceListener(this.LoggingFile, StatisticsManager.TextWriterListenerName);
                Tracer.AddListener(StatisticsManager.StatisticsListenerNamespace, this.TextWriterListener);

                if (shouldInitializeFile)
                {
                    tracer.TraceStatisticsHeader(this.ServiceProvider.GetService<EnvDTE.DTE, EnvDTE.DTE>(), this.ServiceProvider.GetService<SVsExtensionManager, IVsExtensionManager>());
                }
            }
        }

        public void StopCollectingStatistics()
        {
            if (this.TextWriterListener != null && this.ShouldUpload)
            {
                this.TextWriterListener.Flush();
                this.TextWriterListener.Dispose();
                this.TextWriterListener = null;
                Tracer.RemoveListener(StatisticsManager.StatisticsListenerNamespace, StatisticsManager.TextWriterListenerName);
            }
        }

        private void UploadStatistics()
        {
            if (!File.Exists(this.LoggingFile) || !this.ShouldUpload || String.IsNullOrEmpty(this.UploadEndpointURL))
            {
                return;
            }

            this.StopCollectingStatistics();

            try
            {
                using (var wc = new System.Net.WebClient())
                {
                    var resp = wc.UploadFile(this.UploadEndpointURL, "POST", this.LoggingFile);

                    if (Encoding.ASCII.GetString(resp) != "\"ok\"")
                        return;
                }

                File.Delete(this.LoggingFile);
                TimerUploadStatistics.Stop();
                this.LastUpload = DateTime.Now;
            }
            finally
            {
                this.StartCollectingStatistics();
            }
        }

        public IServiceProvider ServiceProvider { get; set; }
    }

    public class StatisticsTextWriterTraceListener : TextWriterTraceListener
    {
        public StatisticsTextWriterTraceListener(string file, string listenerName)
            : base(file, listenerName)
        {
        }

        public override void Write(string message)
        {
            base.Write(DateTime.Now.ToString("s") + "|");
            base.Write(message.Replace("NServiceBusStudio Information", "NServiceBusStudio Info")
                              .Replace(": ", "|"));
        }

        public override void WriteLine(string message)
        {
            base.WriteLine(message);
        }
    }
}
