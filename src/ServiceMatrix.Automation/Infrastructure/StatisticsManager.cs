using Microsoft.VisualStudio.Shell;
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
using NuPattern.Diagnostics;
using NuPattern;

namespace NServiceBusStudio.Automation.Infrastructure
{
    using EnvDTE;

    [Export]
    public class StatisticsManager
    {
        private static readonly ITracer tracer = Tracer.Get<StatisticsManager>();

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

        public static string LoggingFile
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(typeof(StatisticsManager).Assembly.Location), String.Format("{0}-{1}.{2}", Environment.MachineName, Environment.UserName, StatisticsFileExtension));
            }
        }

        public string LastUploadFile
        {
            get { return LoggingFile + ".lastupload"; }
        }

        // Registry Getters/Setters
        public RegistryKey StatisticsKey
        {
            get { return RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(StatisticsRegistryKey); }
        }

        public bool ShouldUpload
        {
            get { return StatisticsKey != null && (string)StatisticsKey.GetValue(UsageDataCollectionApprovedValueName, null) == "1"; }
        }

        public DateTime? LastUpload
        {
            get { return File.Exists(LastUploadFile) ? (DateTime?)DateTime.Parse(File.ReadAllText(LastUploadFile)) : null; }
            set { File.WriteAllText(LastUploadFile, value.ToString()); }
        }

        public string UploadEndpointURL
        {
            get { return (string)StatisticsKey.GetValue(UploadEndpointURLValueName, null); }
        }

        [ImportingConstructor]
        public StatisticsManager([Import(typeof(SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            StartListening();
        }

        private void StartListening()
        {
            StartCollectingStatistics();
            ConfigureStatisticsUpload();
        }

        private void ConfigureStatisticsUpload()
        {
            // Configure Statistics send
            if (!ShouldUpload)
            {
                return;
            }

            TimeSpan nextUpload = default(TimeSpan);
            if (!LastUpload.HasValue)
            {
                // New Solution - Upload in UploadStatisticsInterval
                LastUpload = DateTime.Now;
                nextUpload = UploadStatisticsInterval;
            }
            else if (LastUpload.Value.Add(CleanStatisticsInterval) < DateTime.Now)
            {
                // Clean statistics
                StopCollectingStatistics();
                File.Delete(LoggingFile);
                StartCollectingStatistics();

                // Upload in UploadStatisticsInterval
                LastUpload = DateTime.Now;
                nextUpload = UploadStatisticsInterval;
            }
            else if (LastUpload.Value.Add(UploadStatisticsInterval) < DateTime.Now)
            {
                // Upload in 10 minutes - To avoid do it now!
                nextUpload = new TimeSpan(0, 10, 0);
            }
            else
            {
                // Upload when LastUpload + UploadStatisticsInterval is achieved
                nextUpload = LastUpload.Value.Add(UploadStatisticsInterval) - DateTime.Now;
            }

            TimerUploadStatistics = new Timer();
            TimerUploadStatistics.Interval = nextUpload.TotalMilliseconds;
            TimerUploadStatistics.Start();
            TimerUploadStatistics.Elapsed += (s, e) => UploadStatistics();
        }

        public void StartCollectingStatistics()
        {
            if (TextWriterListener == null && ShouldUpload)
            {
                var shouldInitializeFile = !File.Exists(LoggingFile);

                TextWriterListener = new StatisticsTextWriterTraceListener(LoggingFile, TextWriterListenerName);
                Tracer.Manager.AddListener(StatisticsListenerNamespace, TextWriterListener);

                if (shouldInitializeFile)
                {
                    tracer.TraceStatisticsHeader(ServiceProvider.GetService<DTE, DTE>(), ServiceProvider.GetService<SVsExtensionManager, IVsExtensionManager>());
                }
            }
        }

        public void StopCollectingStatistics()
        {
            if (TextWriterListener != null && ShouldUpload)
            {
                TextWriterListener.Flush();
                TextWriterListener.Dispose();
                Tracer.Manager.RemoveListener(StatisticsListenerNamespace, TextWriterListener);
                TextWriterListener = null;
            }
        }

        private void UploadStatistics()
        {
            if (!File.Exists(LoggingFile) || !ShouldUpload || String.IsNullOrEmpty(UploadEndpointURL))
            {
                return;
            }

            StopCollectingStatistics();

            try
            {
                using (var wc = new WebClient())
                {
                    var resp = wc.UploadFile(UploadEndpointURL, "POST", LoggingFile);

                    if (Encoding.ASCII.GetString(resp) != "\"ok\"")
                        return;
                }

                File.Delete(LoggingFile);
                TimerUploadStatistics.Stop();
                LastUpload = DateTime.Now;
            }
            finally
            {
                StartCollectingStatistics();
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
