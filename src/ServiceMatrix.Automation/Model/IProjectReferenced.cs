namespace NServiceBusStudio
{
    using NuPattern.VisualStudio.Solution;

    public interface IProjectReferenced
    {
        IProject Project { get; }
        string Namespace { get; set; }
    }
}
