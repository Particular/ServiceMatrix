using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using NuGet.VisualStudio;
using NuPattern.Runtime;
using NuPattern.VisualStudio;
using System.IO;


namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC
{
    using NServiceBusStudio.Automation.Extensions;
    using NuPattern.VisualStudio.Solution;

    [DisplayName("Remove Nuget References For SignalR")]
    [Description("Remove Nuget References For SignalR")]
    [CLSCompliant(false)]
    public class RemoveReferencesForSignalR : NuPattern.Runtime.Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }

        [Import]
        public IVsPackageInstaller VsPackageInstaller { get; set; }

        [Import(AllowDefault = true)]
        public ISolution Solution { get; set; }


        [Import]
        public IVsPackageInstallerServices VsPackageInstallerServices { get; set; }


        [Import]
        public IStatusBar StatusBar { get; set; }


        public override void Execute()
        {
            var mvcEndpoint = CurrentElement.As<INServiceBusMVC>();
            if (!mvcEndpoint.IsSignalREnabled)
            {
                var project = CurrentElement.GetProject();
                if (project == null)
                {
                    return;
                }

                // TODO: Remove the Nugetpackages for SignalR

                // Modify the route config
                var filePath = String.Format("{0}\\App_Start\\RouteConfig.cs", mvcEndpoint.Namespace);
                var item = Solution.FindItem(filePath);

                if (item != null)
                {
                    var contents = File.ReadAllText(item.PhysicalPath);
                    //TODO: Remove hack - either add the MapHubs in a new file rather than Modifying RouteConfig or use proper code DOM to locate the function rather than relying that position of the function!
                    // Hack begin - read using a better code dom rather than traversing using {!
                    var newContents = contents.Replace("routes.MapHubs();", "");
                    item.SetContent(newContents);
                    // Hack end
                }
            }
        }
    }
}
