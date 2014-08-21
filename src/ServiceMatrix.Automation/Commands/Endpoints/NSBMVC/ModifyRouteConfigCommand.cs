namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using NServiceBusStudio.Automation.Extensions;
    using NuPattern.Runtime;
    using NuPattern.VisualStudio.Solution;

    [DisplayName("Modify RouteConfig")]
    [Description("Modify RouteConfig to include the hubs in the MVC Endpoint")]
    [CLSCompliant(false)]
   
    public class ModifyRouteConfigCommand : Command
    {
        [Required]
        [Import(AllowDefault = true)]
        public IProductElement CurrentElement { get; set; }


        [Import(AllowDefault = true)]
        public ISolution Solution { get; set; }

        /// <summary>
        /// Gets or sets the Window Factory, used to create a Window Dialog.
        /// </summary>
        [Required]
        [Import(AllowDefault = true)]
        public IDialogWindowFactory WindowFactory { get; set; }

        public override void Execute()
        {
            var mvcEndpoint = CurrentElement.As<INServiceBusMVC>();
            if (!mvcEndpoint.IsSignalREnabled)
            {
                var filePath = String.Format("{0}\\App_Start\\RouteConfig.cs", mvcEndpoint.Namespace);
                var item = Solution.FindItem(filePath);
                
                if (item != null)
                {
                    var contents = File.ReadAllText(item.PhysicalPath);
                    //TODO: Remove hack - either add the MapHubs in a new file rather than Modifying RouteConfig or use proper code DOM to locate the function rather than relying that position of the function!
                    // Hack begin - read using a better code dom rather than traversing using {!
                        var indexToAdd = GetNthIndex(contents, '{', 3);
                        var firstPart = contents.Substring(0, indexToAdd + 1);
                        var stringToAdd = "\t\t\troutes.MapHubs();";
                        var finalPart = contents.Substring(indexToAdd + 1);
                        var newContents = string.Format("{0}\r\n{1}\r\n{2}", firstPart, stringToAdd, finalPart);
                        item.SetContent(newContents);
                    // Hack end
                }
            }
        }

        public int GetNthIndex(string s, char t, int n)
        {
            var count = 0;
            for (var i = 0; i < s.Length; i++)
            {
                if (s[i] == t)
                {
                    count++;
                    if (count == n)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
    }
}
