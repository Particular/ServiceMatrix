namespace NServiceBusStudio
{
    using NServiceBusStudio.Automation.Extensions;
    using NuPattern.VisualStudio.Solution;

    partial interface IEvents : IProjectReferenced
    {
    }

    partial class Events
    {
        public IProject Project
        {
            get { return AsCollection().GetProject(); }
        }

    }
}
