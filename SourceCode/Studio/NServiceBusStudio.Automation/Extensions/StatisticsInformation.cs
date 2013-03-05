using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Extensions
{
    public static class StatisticsInformation
    {
        public static void TraceStatistics(this ITraceSource traceSource, string format, params object[] args)
        {
            traceSource.TraceInformation(String.Format ("{0}|{1}",
                                         DateTime.Now.ToString(),
                                         String.Format (format, args)));
        }

        public static string GetWindowsVersion()
        {
            return String.Format("{0} - {1}",
                                  getOSInfo(),
                                  (Environment.Is64BitOperatingSystem) ? "64 bits" : "32 bits");
        }

        public static string GetVisualStudioVersion(EnvDTE.DTE dte)
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
                    vsVersion = "20010";
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

        private static string getOSInfo()
        {
            //Get Operating system information.
            OperatingSystem os = Environment.OSVersion;
            //Get version information about the os.
            Version vs = os.Version;

            //Variable to hold our return value
            string operatingSystem = "";

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

        private enum SKUEdition
        {
            None = 0,
            Express = 500,
            Standard = 0x3e8,
            Professional = 0x7d0,
            AcademicProfessional = 0x834,
            AcademicStudent = 0x834,
            AcademicStudentMSDNAA = 0x898,
            AcademicEnterprise = 0x8fc,
            Book = 0x960,
            DownloadTrial = 0x9c4,
            Enterprise = 0xbb8,
            VSTO = 0x5dc
        }
    }
}
