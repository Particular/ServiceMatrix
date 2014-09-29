namespace NServiceBusStudio.Automation.Extensions
{
    using System;
    using System.IO;
    using System.Linq;
    using NuPattern.VisualStudio.Solution;

    public static class ItemExtensions
    {
        public static void UpdateContent(this IItem item, Func<string, string> contentUpdate)
        {
            var content = File.ReadAllText(item.PhysicalPath);
            item.SetContent(contentUpdate(content));
        }

        public static IItem FindItem(this ISolution solution, string itemPathTemplate)
        {
            var item = solution.Find<IItem>(string.Format(itemPathTemplate, solution.Name)).FirstOrDefault();
            return item;
        }
    }
}