using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace $safeprojectname$
{
    public static class WebGlobalInitialization
    {
        public static IBus InitializeNServiceBus()
        {
            return NServiceBus.Configure.With()
                .DefaultBuilder()
                .XmlSerializer()
                    .IsTransactional(false)
                    .PurgeOnStartup(false)
                .UnicastBus()
                    .ImpersonateSender(false)
                .CreateBus()
                .Start(() => Configure.Instance.ForInstallationOn<NServiceBus.Installation.Environments.Windows>().Install());
        }
    }
}
