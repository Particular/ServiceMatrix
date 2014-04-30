namespace NServiceBusStudio.Automation.Extensions
{
    using Microsoft.VisualStudio.ExtensionManager;
    using NuPattern.Diagnostics;
    using System;
    using System.Linq;
    using EnvDTE;
    using NuPattern;

    public static class StatisticsInformation
    {
        public static void TraceStatisticsHeader(this ITracer traceSource, DTE dte, IVsExtensionManager extensionManager)
        {
            traceSource.TraceStatistics("========================= NServiceBusStudio General Information =========================");
            traceSource.TraceStatistics("Operating System: {0}", GetOperatingSystemVersion());
            traceSource.TraceStatistics("Machine Name: {0}", Environment.MachineName);
            traceSource.TraceStatistics("UserName: {0}", Environment.UserName);
            traceSource.TraceStatistics("VisualStudio Version: {0}", GetVisualStudioVersion(dte));
            traceSource.TraceStatistics("NuPattern Version: {0}", typeof(Guard).AssemblyQualifiedName);
            traceSource.TraceStatistics("NServiceBusStudio Version: {0}", typeof(StatisticsInformation).Assembly.FullName);
            traceSource.TraceStatistics("Installation Directory: {0}", typeof(StatisticsInformation).Assembly.Location);
            traceSource.TraceStatistics("Other Extensions: {0}", String.Join (";", extensionManager.GetEnabledExtensions().Select (x => String.Format ("{0} ({1})", x.Header.Name, x.Header.Version.ToString()))));
            traceSource.TraceStatistics("=========================================================================================");
        }

        public static void TraceStatistics(this ITracer traceSource, string format, params object[] args)
        {
            traceSource.Info(String.Format (format, args));
        }

        public static string GetOperatingSystemVersion()
        {
            return String.Format("{0} - {1}",
                                  getOSInfo(),
                                  (Environment.Is64BitOperatingSystem) ? "64 bits" : "32 bits");
        }

        public static string getOSInfo()
        {
            //Get Operating system information.
            var os = Environment.OSVersion;
            //Get version information about the os.
            var vs = os.Version;

            //Variable to hold our return value
            var operatingSystem = "";

            if (os.Platform == PlatformID.Win32Windows)
            {
                //This is a pre-NT version of Windows
                switch (vs.Minor)
                {
                    case 0:
                        operatingSystem = "95";
                        break;
                    case 10:
                        if (vs.Revision.ToString() == "2222A")
                            operatingSystem = "98SE";
                        else
                            operatingSystem = "98";
                        break;
                    case 90:
                        operatingSystem = "Me";
                        break;
                    default:
                        break;
                }
            }
            else if (os.Platform == PlatformID.Win32NT)
            {
                switch (vs.Major)
                {
                    case 3:
                        operatingSystem = "NT 3.51";
                        break;
                    case 4:
                        operatingSystem = "NT 4.0";
                        break;
                    case 5:
                        if (vs.Minor == 0)
                            operatingSystem = "2000";
                        else
                            operatingSystem = "XP";
                        break;
                    case 6:
                        if (vs.Minor == 0)
                            operatingSystem = "Vista";
                        else if (vs.Minor == 1)
                            operatingSystem = "7";
                        else
                            operatingSystem = "8";
                        break;
                    default:
                        break;
                }
            }
            //Make sure we actually got something in our OS check
            //We don't want to just return " Service Pack 2" or " 32-bit"
            //That information is useless without the OS version.
            if (operatingSystem != "")
            {
                //Got something.  Let's prepend "Windows" and get more info.
                operatingSystem = "Windows " + operatingSystem;
                //See if there's a service pack installed.
                if (os.ServicePack != "")
                {
                    //Append it to the OS name.  i.e. "Windows XP Service Pack 3"
                    operatingSystem += " " + os.ServicePack;
                }
                //Append the OS architecture.  i.e. "Windows XP Service Pack 3 32-bit"
                //operatingSystem += " " + getOSArchitecture().ToString() + "-bit";
            }
            //Return the information we've gathered.
            return operatingSystem;
        }

        public static string GetVisualStudioVersion(DTE dte)
        {
            var version = new Version(dte.Version);
            var vsVersion = "";
            switch (version.Major)
            {
                case 8:
                    vsVersion = "2005";
                    break;
                case 9:
                    vsVersion = "2008";
                    break;
                case 10:
                    vsVersion = "2010";
                    break;
                case 11:
                    vsVersion = "2012";
                    break;
                default:
                    break;
            }

            return String.Format("{0} {1} {2}",
                                 dte.Name,
                                 vsVersion,
                                 dte.Edition);
        }

        
    }
}
