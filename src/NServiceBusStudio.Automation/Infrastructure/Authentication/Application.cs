using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NServiceBusStudio
{
    public partial interface IApplication
    {
        bool HasAuthentication { get; }
    }

    partial class Application
    {
        public bool HasAuthentication
        {
            get
            {
                return (this.Design.Infrastructure != null && this.Design.Infrastructure.Security != null && this.Design.Infrastructure.Security.Authentication != null);
            }
        }
    }
}
