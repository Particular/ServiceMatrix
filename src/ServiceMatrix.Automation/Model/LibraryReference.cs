namespace NServiceBusStudio
{
    using System.Linq;
    using NuPattern.Runtime;

    partial interface ILibraryReference
    {
        ILibrary Library { get; set; }
        IServiceLibrary ServiceLibrary { get; set; }
    }

    partial class LibraryReference
    {
        partial void Initialize()
        {
            var linked = As<IProductElement>().Root.Traverse().FirstOrDefault(i => i.Id == LibraryId);
            if (linked != null)
            {
                ServiceLibrary = linked.As<IServiceLibrary>();
                Library = linked.As<ILibrary>();
            }
        }

        public ILibrary Library { get; set; }
        public IServiceLibrary ServiceLibrary { get; set; }
    }
}
