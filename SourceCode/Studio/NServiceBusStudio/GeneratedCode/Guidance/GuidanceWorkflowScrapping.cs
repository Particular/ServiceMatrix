
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
                    Tuple.Create ("Getting Started", "Creating A New Project", "http://support.nservicebus.com/customer/portal/articles/856687-getting-started---creating-a-new-project"),
                    Tuple.Create ("Getting Started", "Fault Tolerance", "http://support.nservicebus.com/customer/portal/articles/860511-getting-started---fault-tolerance"),
                    Tuple.Create ("Getting Started", "Publish / Subscribe Communication", "http://support.nservicebus.com/customer/portal/articles/860517--getting-started---publish-subscribe-communication"),
                    Tuple.Create ("Getting Started", "NServiceBus Videos", "http://support.nservicebus.com/customer/portal/articles/910708-nservicebus-videos"),
                    Tuple.Create ("Getting Started", "Overview", "http://support.nservicebus.com/customer/portal/articles/861018-overview"),
                    Tuple.Create ("Getting Started", "Architectural Principles", "http://support.nservicebus.com/customer/portal/articles/861019-architectural-principles"),
                    Tuple.Create ("Getting Started", "Building NServiceBus From Source Files", "http://support.nservicebus.com/customer/portal/articles/861094-building-nservicebus-from-source-files"),
                    Tuple.Create ("Getting Started", "NServiceBus And WCF", "http://support.nservicebus.com/customer/portal/articles/861164-nservicebus-and-wcf"),
                    Tuple.Create ("Getting Started", "NServiceBus And WebSphere/Sonic", "http://support.nservicebus.com/customer/portal/articles/861175-nservicebus-and-websphere-sonic"),
                    Tuple.Create ("Getting Started", "NServiceBus And BizTalk", "http://support.nservicebus.com/customer/portal/articles/861257-nservicebus-and-biztalk"),
                    Tuple.Create ("Day to Day", "Containers", "http://support.nservicebus.com/customer/portal/articles/852357-containers"),
                    Tuple.Create ("Day to Day", "NServiceBus Support For Child Containers", "http://support.nservicebus.com/customer/portal/articles/856108-nservicebus-support-for-child-containers"),
                    Tuple.Create ("Day to Day", "Logging In NServiceBus v.3", "http://support.nservicebus.com/customer/portal/articles/856253-logging-in-nservicebus-v-3"),
                    Tuple.Create ("Day to Day", "Messages As Intefaces", "http://support.nservicebus.com/customer/portal/articles/856259-messages-as-intefaces"),
                    Tuple.Create ("Day to Day", "Introducing IEvent And ICommand", "http://support.nservicebus.com/customer/portal/articles/856271-introducing-ievent-and-icommand"),
                    Tuple.Create ("Day to Day", "Staying Updated With NuGet", "http://support.nservicebus.com/customer/portal/articles/856306-staying-updated-with-nuget"),
                    Tuple.Create ("Day to Day", "Unobtrusive Mode Messages", "http://support.nservicebus.com/customer/portal/articles/856287--unobtrusive-mode-messages"),
                    Tuple.Create ("Day to Day", "Unit Testing", "http://support.nservicebus.com/customer/portal/articles/856297-unit-testing"),
                    Tuple.Create ("Day to Day", "One Way/Send Only Endpoints", "http://support.nservicebus.com/customer/portal/articles/856305-one-way-send-only-endpoints"),
                    Tuple.Create ("Day to Day", "Scheduling With NServiceBus", "http://support.nservicebus.com/customer/portal/articles/856589--scheduling-with-nservicebus"),
                    Tuple.Create ("Day to Day", "Second Level Retries", "http://support.nservicebus.com/customer/portal/articles/856620-second-level-retries"),
                    Tuple.Create ("Day to Day", "NServiceBus Installers", "http://support.nservicebus.com/customer/portal/articles/856643-nservicebus-installers"),
                    Tuple.Create ("Day to Day", "Convention Over Configuration", "http://support.nservicebus.com/customer/portal/articles/864341-convention-over-configuration"),
                    Tuple.Create ("Day to Day", "Managing NServiceBus Using PowerShell", "http://support.nservicebus.com/customer/portal/articles/957892-managing-nservicebus-using-powershell"),
                    Tuple.Create ("Hosting", "The NServiceBus Host", "http://support.nservicebus.com/customer/portal/articles/856698-the-nservicebus-host"),
                    Tuple.Create ("Hosting", "Hosting NServiceBus In Your Own Process", "http://support.nservicebus.com/customer/portal/articles/852419-hosting-nservicebus-in-your-own-process"),
                    Tuple.Create ("Hosting", "Profiles For NServiceBus Host", "http://support.nservicebus.com/customer/portal/articles/859283-profiles-for-nservicebus-host"),
                    Tuple.Create ("Hosting", "More On Profiles", "http://support.nservicebus.com/customer/portal/articles/859299-more-on-profiles"),
                    Tuple.Create ("Hosting", "NServiceBus 32-bit (x86) Host Process", "http://support.nservicebus.com/customer/portal/articles/859326-nservicebus-32-bit-x86-host-process"),
                    Tuple.Create ("Persistence In NServiceBus", "Persistence In NServiceBus", "http://support.nservicebus.com/customer/portal/articles/859337-persistence-in-nservicebus"),
                    Tuple.Create ("Persistence In NServiceBus", "Using RavenDB In NServiceBus - Installing", "http://support.nservicebus.com/customer/portal/articles/859351-using-ravendb-in-nservicebus-%E2%80%93-installing"),
                    Tuple.Create ("Persistence In NServiceBus", "Using RavenDB In NServiceBus - Connecting", "http://support.nservicebus.com/customer/portal/articles/859362-using-ravendb-in-nservicebus-%E2%80%93-connecting"),
                    Tuple.Create ("Persistence In NServiceBus", "Relational Persistence Using NHibernate", "http://support.nservicebus.com/customer/portal/articles/859368-relational-persistence-using-nhibernate"),
                    Tuple.Create ("Management And Monitoring", "Auditing With NServiceBus", "http://support.nservicebus.com/customer/portal/articles/859424-auditing-with-nservicebus"),
                    Tuple.Create ("Management And Monitoring", "Monitoring NServiceBus Endpoints", "http://support.nservicebus.com/customer/portal/articles/859446-monitoring-nservicebus-endpoints"),
                    Tuple.Create ("Management And Monitoring", "MSMQ Information", "http://support.nservicebus.com/customer/portal/articles/859546-msmq-information"),
                    Tuple.Create ("Scaling Out", "Performance", "http://support.nservicebus.com/customer/portal/articles/859547-performance"),
                    Tuple.Create ("Scaling Out", "The Gateway And Multi-Site Distribution", "http://support.nservicebus.com/customer/portal/articles/859548-the-gateway-and-multi-site-distribution"),
                    Tuple.Create ("Scaling Out", "Load Balancing With The Distributor", "http://support.nservicebus.com/customer/portal/articles/859556-load-balancing-with-the-distributor"),
                    Tuple.Create ("Scaling Out", "Unit Of Work In NServiceBus", "http://support.nservicebus.com/customer/portal/articles/860275-unit-of-work-in-nservicebus"),
                    Tuple.Create ("Scaling Out", "Unit Of Work Implementation For RavenDB", "http://support.nservicebus.com/customer/portal/articles/860276-unit-of-work-implementation-for-ravendb"),
                    Tuple.Create ("Scaling Out", "Introduction To The Gateway", "http://support.nservicebus.com/customer/portal/articles/933160-introduction-to-the-gateway"),
                    Tuple.Create ("Scaling Out", "Deploying NServiceBus In A Windows Failover Cluster", "http://support.nservicebus.com/customer/portal/articles/965131-deploying-nservicebus-in-a-windows-failover-cluster"),
                    Tuple.Create ("Publish-Subscribe", "How Pub/Sub Works", "http://support.nservicebus.com/customer/portal/articles/860297-how-pub-sub-works"),
                    Tuple.Create ("Publish-Subscribe", "Publish/Subscribe Configuration", "http://support.nservicebus.com/customer/portal/articles/860436-publish-subscribe-configuration"),
                    Tuple.Create ("Long-Running Processes", "Sagas In NServiceBus", "http://support.nservicebus.com/customer/portal/articles/860458-sagas-in-nservicebus"),
                    Tuple.Create ("Long-Running Processes", "NServiceBus Sagas And Concurrency", "http://support.nservicebus.com/customer/portal/articles/860490-nservicebus-sagas-and-concurrency"),
                    Tuple.Create ("Customization", "Customizing NServiceBus Configuration", "http://support.nservicebus.com/customer/portal/articles/860491-customizing-nservicebus-configuration"),
                    Tuple.Create ("Customization", "Pipeline Management Using Message Mutators", "http://support.nservicebus.com/customer/portal/articles/860492-pipeline-management-using-message-mutators"),
                    Tuple.Create ("Versioning", "Migrating To NServiceBus 3.0 â€“ Timeouts", "http://support.nservicebus.com/customer/portal/articles/860494-migrating-to-nservicebus-3-0-%E2%80%93-timeouts"),
                    Tuple.Create ("Versioning", "Potential Issues Updating NSB Studio On Visual Studio 11 Beta", "http://support.nservicebus.com/customer/portal/articles/860500-potential-issues-updating-nsb-studio-on-visual-studio-11-beta"),
                    Tuple.Create ("FAQ", "MsmqTransportConfig", "http://support.nservicebus.com/customer/portal/articles/862362-msmqtransportconfig"),
                    Tuple.Create ("FAQ", "How Do I Define A Message?", "http://support.nservicebus.com/customer/portal/articles/862367-how-do-i-define-a-message-"),
                    Tuple.Create ("FAQ", "How Do I Discard Old Messages?", "http://support.nservicebus.com/customer/portal/articles/862375-how-do-i-discard-old-messages-"),
                    Tuple.Create ("FAQ", "How Do I Instantiate A Message?", "http://support.nservicebus.com/customer/portal/articles/862384-how-do-i-instantiate-a-message-"),
                    Tuple.Create ("FAQ", "How Do I Send A Message?", "http://support.nservicebus.com/customer/portal/articles/862385-how-do-i-send-a-message-"),
                    Tuple.Create ("FAQ", "How Do I Specify To Which Destination A Message Will Be Sent?", "http://support.nservicebus.com/customer/portal/articles/862387-how-do-i-specify-to-which-destination-a-message-will-be-sent-"),
                    Tuple.Create ("FAQ", "How Can I See The Queues And Messages On A Machine?", "http://support.nservicebus.com/customer/portal/articles/862390-how-can-i-see-the-queues-and-messages-on-a-machine-"),
                    Tuple.Create ("FAQ", "How Do I Handle A Message?", "http://support.nservicebus.com/customer/portal/articles/862391-how-do-i-handle-a-message-"),
                    Tuple.Create ("FAQ", "How Do I Specify The Order In Which Handlers Are Invoked?", "http://support.nservicebus.com/customer/portal/articles/862397-how-do-i-specify-the-order-in-which-handlers-are-invoked-"),
                    Tuple.Create ("FAQ", "How Do I Get A Reference To IBus In My Message Handler?", "http://support.nservicebus.com/customer/portal/articles/862398-how-do-i-get-a-reference-to-ibus-in-my-message-handler-"),
                    Tuple.Create ("FAQ", "How Do I Get Technical Information About A Message?", "http://support.nservicebus.com/customer/portal/articles/862399-how-do-i-get-technical-information-about-a-message-"),
                    Tuple.Create ("FAQ", "How Do I Reply To A Message?", "http://support.nservicebus.com/customer/portal/articles/862405-how-do-i-reply-to-a-message-"),
                    Tuple.Create ("FAQ", "How Do I Handle Responses At Client-side?", "http://support.nservicebus.com/customer/portal/articles/862406-how-do-i-handle-responses-at-client-side-"),
                    Tuple.Create ("FAQ", "How Do I Handle Responses At Client-side?", "http://support.nservicebus.com/customer/portal/articles/862408-how-do-i-handle-responses-at-client-side-"),
                    Tuple.Create ("FAQ", "How Do I Publish A Message?", "http://support.nservicebus.com/customer/portal/articles/862409-how-do-i-publish-a-message-"),
                    Tuple.Create ("FAQ", "How Do I Handle Exceptions?", "http://support.nservicebus.com/customer/portal/articles/862410-how-do-i-handle-exceptions-"),
                    Tuple.Create ("FAQ", "How Do I Expose An NServiceBus Endpoint As A Web/Wcf Service?", "http://support.nservicebus.com/customer/portal/articles/862411-how-do-i-expose-an-nservicebus-endpoint-as-a-web-wcf-service-"),
                    Tuple.Create ("FAQ", "Type Was Not Registered In The Serializer", "http://support.nservicebus.com/customer/portal/articles/862413-type-was-not-registered-in-the-serializer"),
                    Tuple.Create ("FAQ", "MessageQueueException: Insufficient resources to perform operation", "http://support.nservicebus.com/customer/portal/articles/862414-messagequeueexception-insufficient-resources-to-perform-operation"),
                    Tuple.Create ("FAQ", "How To Specify Your Input Queue Name?", "http://support.nservicebus.com/customer/portal/articles/862420-how-to-specify-your-input-queue-name-"),
                    Tuple.Create ("FAQ", "RunMeFirst.bat Throws An Exception", "http://support.nservicebus.com/customer/portal/articles/862429-runmefirst-bat-throws-an-exception"),
                    Tuple.Create ("FAQ", "No endpoint Configuration Found In Scanned Assemblies Exception", "http://support.nservicebus.com/customer/portal/articles/864243-no-endpoint-configuration-found-in-scanned-assemblies-exception"),
                    Tuple.Create ("FAQ", "DTCPIng WARNING: the CID values for both test machines are the same", "http://support.nservicebus.com/customer/portal/articles/864249-dtcping-warning-the-cid-values-for-both-test-machines-are-the-same"),
                    Tuple.Create ("FAQ", "Configuring AWS For NServiceBus", "http://support.nservicebus.com/customer/portal/articles/864261-configuring-aws-for-nservicebus"),
                    Tuple.Create ("FAQ", "Licensing And Distribution", "http://support.nservicebus.com/customer/portal/articles/864262-licensing-and-distribution"),
                    Tuple.Create ("FAQ", "How To Debug RavenDB Through Fiddler Using NServiceBus", "http://support.nservicebus.com/customer/portal/articles/864340-how-to-debug-ravendb-through-fiddler-using-nservicebus"),
                    Tuple.Create ("FAQ", "How Do I Centralize All Unobtrusive Declarations?", "http://support.nservicebus.com/customer/portal/articles/870736-how-do-i-centralize-all-unobtrusive-declarations-"),
                    Tuple.Create ("FAQ", "DefiningMessagesAs And Exception When Starting Endpoint", "http://support.nservicebus.com/customer/portal/articles/910754-definingmessagesas-and-exception-when-starting-endpoint"),
                    Tuple.Create ("FAQ", "RunMeFirst.bat Hangs", "http://support.nservicebus.com/customer/portal/articles/919267-runmefirst-bat-hangs"),
                    Tuple.Create ("FAQ", "Ho Do I Reduce Throughput Of an Endpoint?", "http://support.nservicebus.com/customer/portal/articles/924602-ho-do-i-reduce-throughput-of-an-endpoint-"),
                    Tuple.Create ("FAQ", "Does NServiceBus Supports Both Native And Non-Native Connectivity?", "http://support.nservicebus.com/customer/portal/articles/940521-does-nservicebus-supports-both-native-and-non-native-connectivity-"),
                    Tuple.Create ("FAQ", "InvalidOperationException In Unobtrusive Mode", "http://support.nservicebus.com/customer/portal/articles/975934-invalidoperationexception-in-unobtrusive-mode"),
                    Tuple.Create ("Samples", "Full Duplex Sample v.3", "http://support.nservicebus.com/customer/portal/articles/894137-full-duplex-sample-v-3"),
                    Tuple.Create ("Samples", "Publish/Subscribe Sample", "http://support.nservicebus.com/customer/portal/articles/894145-publish-subscribe-sample"),
                    Tuple.Create ("Samples", "Unobtrusive Sample", "http://support.nservicebus.com/customer/portal/articles/894010-unobtrusive-sample"),
                    Tuple.Create ("Samples", "Scale Out Sample", "http://support.nservicebus.com/customer/portal/articles/894157-scale-out-sample"),
                    Tuple.Create ("Samples", "Using NServiceBus in a ASP.NET Web Application", "http://support.nservicebus.com/customer/portal/articles/893989-using-nservicebus-in-a-asp-net-web-application"),
                    Tuple.Create ("Samples", "Using NServiceBus with ASP.NET MVC", "http://support.nservicebus.com/customer/portal/articles/894008-using-nservicebus-with-asp-net-mvc"),
                    Tuple.Create ("Samples", "Injecting The Bus Into Asp.Net MVC Controller", "http://support.nservicebus.com/customer/portal/articles/894123-injecting-the-bus-into-asp-net-mvc-controller"),
                    Tuple.Create ("Samples", "Encryption Sample", "http://support.nservicebus.com/customer/portal/articles/894131-encryption-sample"),
                    Tuple.Create ("Samples", "Generic Host Sample", "http://support.nservicebus.com/customer/portal/articles/894138-generic-host-sample"),
                    Tuple.Create ("Samples", "Versioning Sample", "http://support.nservicebus.com/customer/portal/articles/894151-versioning-sample"),
                    Tuple.Create ("Samples", "NServiceBus Message Mutators Sample", "http://support.nservicebus.com/customer/portal/articles/894155-nservicebus-message-mutators-sample"),
                    Tuple.Create ("Samples", "Attachments / DataBus Sample", "http://support.nservicebus.com/customer/portal/articles/894156-attachments-databus-sample"),
                };
            }
        }


        public Initial Generate()
        {
            Join previousJoin = null;
            var initial = new Initial
            {
                Name = "NServiceBus Studio Guidance",
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
