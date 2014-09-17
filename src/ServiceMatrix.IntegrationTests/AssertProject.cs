namespace ServiceMatrix.IntegrationTests
{
    using System;
    using System.Linq;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using NuPattern.VisualStudio.Solution;

    public static class AssertProject
    {
        public static void HasFolder<T>(T container, string folderName, params Action<IFolder>[] folderAsserts)
            where T : IFolderContainer, IItemContainer
        {
            var folder = container.Folders.FirstOrDefault(f => f.Name == folderName);
            Assert.IsNotNull(folder, "Cannot find folder named '{0}' on '{1}'", folderName, container.Name);
            foreach (var folderAssert in folderAsserts)
            {
                folderAssert(folder);
            }
        }

        public static void HasItem<T>(T container, string itemName, params Action<IItemContainer>[] itemAsserts)
            where T : IItemContainer
        {
            var item = container.Items.FirstOrDefault(i => i.Name == itemName);
            Assert.IsNotNull(item, "Cannot find item named '{0}' on '{1}'", itemName, container.Name);
            foreach (var itemAssert in itemAsserts)
            {
                itemAssert(item);
            }
        }
    }
}