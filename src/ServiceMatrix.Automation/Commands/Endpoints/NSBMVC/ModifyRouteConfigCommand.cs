namespace NServiceBusStudio.Automation.Commands.Endpoints.NSBMVC
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.Composition;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Linq;
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
            var component = CurrentElement.As<NServiceBusStudio.IComponent>();
            var mvcEndpoint = NServiceBusStudio.Automation.Model.Helpers.GetMvcEndpointFromLinkedElement(CurrentElement);
            var filePath = String.Format("{0}\\App_Start\\RouteConfig.cs", mvcEndpoint.Namespace);
            var item = Solution.FindItem(filePath);            
            if (item != null)
            {
                var contents = File.ReadAllText(item.PhysicalPath);
                
                // Hack begin
                var indexToAdd = GetNthIndex(contents, '{', 3);
                var firstPart = contents.Substring(0, indexToAdd + 1);
                var stringToAdd = "routes.MapHubs();";
                var finalPart = contents.Substring(indexToAdd + 1);
                var newContents = string.Format("{0}\r\n{1}\r\n{2}", firstPart, stringToAdd, finalPart);
                // Hack end

                item.SetContent(newContents);
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
