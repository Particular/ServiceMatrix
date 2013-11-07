
using NuPattern.Runtime.Guidance.Workflow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Guidance
{
    public class GuidanceWorkflowScrapping
    {
        private List<Tuple<string, string, string>> Items
        {
            get
            {
                return new List<Tuple<string, string, string>>()
                {
                    Tuple.Create ("Getting Started", "Creating A New Project", "http://particular.net/articles/getting-started---creating-a-new-project"),
                    Tuple.Create ("Getting Started", "Fault Tolerance", "http://particular.net/articles/getting-started---fault-tolerance"),
                    Tuple.Create ("Getting Started", "Publish / Subscribe Communication", "http://particular.net/articles/getting-started---publish-subscribe-communication"),
                    Tuple.Create ("Getting Started", "NServiceBus Videos", "http://particular.net/articles/nservicebus-videos"),
                    Tuple.Create ("Getting Started", "Overview", "http://particular.net/articles/overview"),
                    Tuple.Create ("Getting Started", "Architectural Principles", "http://particular.net/articles/architectural-principles"),
                    Tuple.Create ("Getting Started", "Building NServiceBus From Source Files", "http://particular.net/articles/building-nservicebus-from-source-files"),
                    Tuple.Create ("Getting Started", "NServiceBus And WCF", "http://particular.net/articles/nservicebus-and-wcf"),
                    Tuple.Create ("Getting Started", "NServiceBus And WebSphere/Sonic", "http://particular.net/articles/nservicebus-and-websphere-sonic"),
                    Tuple.Create ("Getting Started", "NServiceBus And BizTalk", "http://particular.net/articles/nservicebus-and-biztalk"),
                    Tuple.Create ("Day to Day", "Containers", "http://particular.net/articles/containers"),
                    Tuple.Create ("Day to Day", "NServiceBus Support For Child Containers", "http://particular.net/articles/nservicebus-support-for-child-containers"),
                    Tuple.Create ("Day to Day", "Logging In NServiceBus v.3", "http://particular.net/articles/logging-in-nservicebus"),
                    Tuple.Create ("Day to Day", "Messages As Intefaces", "http://particular.net/articles/messages-as-interfaces"),
                    Tuple.Create ("Day to Day", "Introducing IEvent And ICommand", "http://particular.net/articles/introducing-ievent-and-icommand"),
                    Tuple.Create ("Day to Day", "Staying Updated With NuGet", "http://particular.net/articles/staying-updated-with-nuget"),
                    Tuple.Create ("Day to Day", "Unobtrusive Mode Messages", "http://particular.net/articles/unobtrusive-mode-messages"),
                    Tuple.Create ("Day to Day", "Unit Testing", "http://particular.net/articles/unit-testing"),
                    Tuple.Create ("Day to Day", "One Way/Send Only Endpoints", "http://particular.net/articles/one-way-send-only-endpoints"),
                    Tuple.Create ("Day to Day", "Scheduling With NServiceBus", "http://particular.net/articles/scheduling-with-nservicebus"),
                    Tuple.Create ("Day to Day", "Second Level Retries", "http://particular.net/articles/second-level-retries"),
                    Tuple.Create ("Day to Day", "NServiceBus Installers", "http://particular.net/articles/nservicebus-installers"),
                    Tuple.Create ("Day to Day", "Convention Over Configuration", "http://particular.net/articles/convention-over-configuration"),
                    Tuple.Create ("Day to Day", "Managing NServiceBus Using PowerShell", "http://particular.net/articles/managing-nservicebus-using-powershell"),
                    Tuple.Create ("Hosting", "The NServiceBus Host", "http://particular.net/articles/the-nservicebus-host"),
                    Tuple.Create ("Hosting", "Hosting NServiceBus In Your Own Process", "http://particular.net/articles/hosting-nservicebus-in-your-own-process"),
                    Tuple.Create ("Hosting", "Profiles For NServiceBus Host", "http://particular.net/articles/profiles-for-nservicebus-host"),
                    Tuple.Create ("Hosting", "More On Profiles", "http://particular.net/articles/more-on-profiles"),
                    Tuple.Create ("Hosting", "NServiceBus 32-bit (x86) Host Process", "http://particular.net/articles/nservicebus-32-bit-x86-host-process"),
                    Tuple.Create ("Persistence In NServiceBus", "Persistence In NServiceBus", "http://particular.net/articles/persistence-in-nservicebus"),
                    Tuple.Create ("Persistence In NServiceBus", "Using RavenDB In NServiceBus - Installing", "http://particular.net/articles/using-ravendb-in-nservicebus-installing"),
                    Tuple.Create ("Persistence In NServiceBus", "Using RavenDB In NServiceBus - Connecting", "http://particular.net/articles/using-ravendb-in-nservicebus-connecting"),
                    Tuple.Create ("Persistence In NServiceBus", "Relational Persistence Using NHibernate", "http://particular.net/articles/relational-persistence-using-nhibernate"),
                    Tuple.Create ("Management And Monitoring", "Auditing With NServiceBus", "http://particular.net/articles/auditing-with-nservicebus"),
                    Tuple.Create ("Management And Monitoring", "Monitoring NServiceBus Endpoints", "http://particular.net/articles/monitoring-nservicebus-endpoints"),
                    Tuple.Create ("Management And Monitoring", "MSMQ Information", "http://particular.net/articles/msmq-information"),
                    Tuple.Create ("Scaling Out", "Performance", "http://particular.net/articles/performance"),
                    Tuple.Create ("Scaling Out", "The Gateway And Multi-Site Distribution", "http://particular.net/articles/the-gateway-and-multi-site-distribution"),
                    Tuple.Create ("Scaling Out", "Load Balancing With The Distributor", "http://particular.net/articles/load-balancing-with-the-distributor"),
                    Tuple.Create ("Scaling Out", "Unit Of Work In NServiceBus", "http://particular.net/articles/unit-of-work-in-nservicebus"),
                    Tuple.Create ("Scaling Out", "Unit Of Work Implementation For RavenDB", "http://particular.net/articles/unit-of-work-implementation-for-ravendb"),
                    Tuple.Create ("Scaling Out", "Introduction To The Gateway", "http://particular.net/articles/introduction-to-the-gateway"),
                    Tuple.Create ("Scaling Out", "Deploying NServiceBus In A Windows Failover Cluster", "http://particular.net/articles/deploying-nservicebus-in-a-windows-failover-cluster"),
                    Tuple.Create ("Publish-Subscribe", "How Pub/Sub Works", "http://particular.net/articles/how-pub-sub-works"),
                    Tuple.Create ("Publish-Subscribe", "Publish/Subscribe Configuration", "http://particular.net/articles/publish-subscribe-configuration"),
                    Tuple.Create ("Long-Running Processes", "Sagas In NServiceBus", "http://particular.net/articles/sagas-in-nservicebus"),
                    Tuple.Create ("Long-Running Processes", "NServiceBus Sagas And Concurrency", "http://particular.net/articles/nservicebus-sagas-and-concurrency"),
                    Tuple.Create ("Customization", "Customizing NServiceBus Configuration", "http://particular.net/articles/customizing-nservicebus-configuration"),
                    Tuple.Create ("Customization", "Pipeline Management Using Message Mutators", "http://particular.net/articles/pipeline-management-using-message-mutators"),
                    Tuple.Create ("Versioning", "Migrating To NServiceBus 3.0 - Timeouts", "http://particular.net/articles/migrating-to-nservicebus-3.0-–-timeouts"),
                    Tuple.Create ("Versioning", "Potential Issues Updating NSB Studio On Visual Studio 11 Beta", "http://particular.net/articles/potential-issues-updating-nsb-studio-on-visual-studio-11-beta"),
                    Tuple.Create ("FAQ", "MsmqTransportConfig", "http://particular.net/articles/msmqtransportconfig"),
                    Tuple.Create ("FAQ", "How Do I Define A Message?", "http://particular.net/articles/how-do-i-define-a-message"),
                    Tuple.Create ("FAQ", "How Do I Discard Old Messages?", "http://particular.net/articles/how-do-i-discard-old-messages"),
                    Tuple.Create ("FAQ", "How Do I Instantiate A Message?", "http://particular.net/articles/how-do-i-instantiate-a-message"),
                    Tuple.Create ("FAQ", "How Do I Send A Message?", "http://particular.net/articles/how-do-i-send-a-message"),
                    Tuple.Create ("FAQ", "How Do I Specify To Which Destination A Message Will Be Sent?", "http://particular.net/articles/how-do-i-specify-store-forward-for-a-message"),
                    Tuple.Create ("FAQ", "How Can I See The Queues And Messages On A Machine?", "http://particular.net/articles/how-can-i-see-the-queues-and-messages-on-a-machine"),
                    Tuple.Create ("FAQ", "How Do I Handle A Message?", "http://particular.net/articles/how-do-i-handle-a-message"),
                    Tuple.Create ("FAQ", "How Do I Specify The Order In Which Handlers Are Invoked?", "http://particular.net/articles/how-do-i-specify-the-order-in-which-handlers-are-invoked"),
                    Tuple.Create ("FAQ", "How Do I Get A Reference To IBus In My Message Handler?", "http://particular.net/articles/how-do-i-get-a-reference-to-ibus-in-my-message-handler"),
                    Tuple.Create ("FAQ", "How Do I Get Technical Information About A Message?", "http://particular.net/articles/how-do-i-get-technical-information-about-a-message"),
                    Tuple.Create ("FAQ", "How Do I Reply To A Message?", "http://particular.net/articles/how-do-i-reply-to-a-message"),
                    Tuple.Create ("FAQ", "How Do I Handle Responses At Client-side?", "http://particular.net/articles/how-do-i-handle-responses-on-the-client-side"),
                    Tuple.Create ("FAQ", "How Do I Publish/Subscribe to a Message?", "http://particular.net/articles/how-to-pub/sub-with-NServiceBus"),
                    Tuple.Create ("FAQ", "How Do I Handle Exceptions?", "http://particular.net/articles/how-do-i-handle-exceptions"),
                    Tuple.Create ("FAQ", "How Do I Expose An NServiceBus Endpoint As A Web/Wcf Service?", "http://particular.net/articles/how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service"),
                    Tuple.Create ("FAQ", "Type Was Not Registered In The Serializer", "http://particular.net/articles/type-was-not-registered-in-the-serializer"),
                    Tuple.Create ("FAQ", "MessageQueueException: Insufficient resources to perform operation", "http://particular.net/articles/messagequeueexception-insufficient-resources-to-perform-operation"),
                    Tuple.Create ("FAQ", "How To Specify Your Input Queue Name?", "http://particular.net/articles/how-to-specify-your-input-queue-name"),
                    Tuple.Create ("FAQ", "RunMeFirst.bat Throws An Exception", "http://particular.net/articles/runmefirst.bat-throws-an-exception"),
                    Tuple.Create ("FAQ", "No endpoint Configuration Found In Scanned Assemblies Exception", "http://particular.net/articles/no-endpoint-configuration-found-in-scanned-assemblies-exception"),
                    Tuple.Create ("FAQ", "DTCPIng WARNING: the CID values for both test machines are the same", "http://particular.net/articles/dtcping-warning-the-cid-values-for-both-test-machines-are-the-same"),
                    Tuple.Create ("FAQ", "Configuring AWS For NServiceBus", "http://particular.net/articles/configuring-aws-for-nservicebus"),
                    Tuple.Create ("FAQ", "Licensing And Distribution", "http://particular.net/articles/licensing-and-distribution"),
                    Tuple.Create ("FAQ", "How To Debug RavenDB Through Fiddler Using NServiceBus", "http://particular.net/articles/how-to-debug-ravendb-through-fiddler-using-nservicebus"),
                    Tuple.Create ("FAQ", "How Do I Centralize All Unobtrusive Declarations?", "http://particular.net/articles/how-do-i-centralize-all-unobtrusive-declarations"),
                    Tuple.Create ("FAQ", "DefiningMessagesAs And Exception When Starting Endpoint", "http://particular.net/articles/definingmessagesas-and-exception-when-starting-endpoint"),
                    Tuple.Create ("FAQ", "RunMeFirst.bat Hangs", "http://particular.net/articles/runmefirst.bat-hangs"),
                    Tuple.Create ("FAQ", "Ho Do I Reduce Throughput Of an Endpoint?", "http://particular.net/articles/how-to-reduce-throughput-of-an-endpoint"),
                    Tuple.Create ("FAQ", "Does NServiceBus Supports Both Native And Non-Native Connectivity?", "http://particular.net/articles/does-nservicebus-support-both-native-and-non-native-connectivity"),
                    Tuple.Create ("FAQ", "InvalidOperationException In Unobtrusive Mode", "http://particular.net/articles/invalidoperationexception-in-unobtrusive-mode"),
                    Tuple.Create ("Samples", "Full Duplex Sample v.3", "http://particular.net/articles/full-duplex-sample-v3"),
                    Tuple.Create ("Samples", "Publish/Subscribe Sample", "http://particular.net/articles/publish-subscribe-sample"),
                    Tuple.Create ("Samples", "Unobtrusive Sample", "http://particular.net/articles/unobtrusive-sample"),
                    Tuple.Create ("Samples", "Scale Out Sample", "http://particular.net/articles/scale-out-sample"),
                    Tuple.Create ("Samples", "Using NServiceBus in a ASP.NET Web Application", "http://particular.net/articles/using-nservicebus-in-a-asp.net-web-application"),
                    Tuple.Create ("Samples", "Using NServiceBus with ASP.NET MVC", "http://particular.net/articles/using-nservicebus-with-asp.net-mvc"),
                    Tuple.Create ("Samples", "Injecting The Bus Into Asp.Net MVC Controller", "http://particular.net/articles/injecting-the-bus-into-asp.net-mvc-controller"),
                    Tuple.Create ("Samples", "Encryption Sample", "http://particular.net/articles/encryption-sample"),
                    Tuple.Create ("Samples", "Generic Host Sample", "http://particular.net/articles/generic-host-sample"),
                    Tuple.Create ("Samples", "Versioning Sample", "http://particular.net/articles/versioning-sample"),
                    Tuple.Create ("Samples", "NServiceBus Message Mutators Sample", "http://particular.net/articles/nservicebus-message-mutators-sample"),
                    Tuple.Create ("Samples", "Attachments / DataBus Sample", "http://particular.net/articles/attachments-databus-sample"),
                };
            }
        }


        public Initial Generate()
        {
            Join previousJoin = null;
            var initial = new Initial
            {
                Name = "ServiceMatrix Guidance",
            };

            foreach (var topic in this.Items.GroupBy(x => x.Item1))
            {
                var fork = new Fork
                {
                    Name = topic.Key.Trim(),
                };
                if (previousJoin == null)
                    initial.ConnectTo(fork);
                else
                    previousJoin.ConnectTo(fork);

                var lastItem = this.GenerateItems(topic, fork);


                var join = new Join
                {
                    Name = topic.Key + "Join",
                };
                lastItem.ConnectTo(join);
                previousJoin = join;
            }

            var final = new Final
            {
                Name = "Final",
            };
            previousJoin.ConnectTo(final);

            return initial;
        }

        private GuidanceAction GenerateItems(IGrouping<string, Tuple<string, string, string>> topic, Fork fork)
        {
            var previousGuidanceAction = default(GuidanceAction);

            foreach (var item in topic)
            {
                var guidanceaction = new GuidanceAction
                {
                    Name = item.Item2.Trim(),
                    Link = item.Item3,
                };
                if (previousGuidanceAction == null)
                    fork.ConnectTo(guidanceaction);
                else
                    previousGuidanceAction.ConnectTo(guidanceaction);
                previousGuidanceAction = guidanceaction;
            }

            return previousGuidanceAction;
        }


    }
}
