namespace ServiceMatrix.IntegrationTests
{
    using System;
    using System.IO;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NuPattern.VisualStudio.Solution;

    public static class ItemExtensions
    {
        public static void UpdateContent(this IItem item, Func<string, string> contentUpdate)
        {
            Assert.IsNotNull(item);

            var content = File.ReadAllText(item.PhysicalPath);
            item.SetContent(contentUpdate(content));
        }

        public static IItem FindItem(this ISolution solution, string itemPathTemplate)
        {
            var item = solution.Find<IItem>(string.Format(itemPathTemplate, solution.Name)).FirstOrDefault();
            Assert.IsNotNull(item, "item not found with path " + itemPathTemplate);
            return item;
        }
    }
}