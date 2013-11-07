using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NuPattern.Runtime;

namespace NServiceBusStudio
{
    partial interface ILibraryReference
    {
        ILibrary Library { get; set; }
        IServiceLibrary ServiceLibrary { get; set; }
    }

    partial class LibraryReference
    {
        partial void Initialize()
        {
            var linked = this.As<IProductElement>().Root.Traverse().FirstOrDefault(i => i.Id == this.LibraryId);
            if (linked != null)
            {
                this.ServiceLibrary = linked.As<IServiceLibrary>();
                this.Library = linked.As<ILibrary>();
            }
        }

        public ILibrary Library { get; set; }
        public IServiceLibrary ServiceLibrary { get; set; }
    }
}
