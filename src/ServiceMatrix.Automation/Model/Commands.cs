namespace NServiceBusStudio
{
    using NServiceBusStudio.Automation.Extensions;
    using NuPattern.VisualStudio.Solution;

    partial interface ICommands : IProjectReferenced
    {
    }

    partial class Commands
    {
        public IProject Project
        {
            get { return AsCollection().GetProject(); }
        }
    }
}
