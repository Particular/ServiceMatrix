namespace NServiceBusStudio
{
    public interface IRenameRefactoringNamespace
    {
        string OriginalInstanceName { get; }
        string InstanceName { get; }
    }
}
