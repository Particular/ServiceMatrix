using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace $safeprojectname$.Infrastructure
{
    public static class WebGlobalInitialization
    {
        public static IBus InitializeNServiceBus()
        {
            return NServiceBus.Configure.With()
                .Log4Net()
                .DefaultBuilder()
                .ForMvc()
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(false)
                    .PurgeOnStartup(false)
                .UnicastBus()
                    .ImpersonateSender(false)
                .CreateBus()
                .Start();
        }
    }
}