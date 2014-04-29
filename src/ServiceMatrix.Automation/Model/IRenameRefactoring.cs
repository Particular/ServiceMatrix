namespace NServiceBusStudio
{
    using System.Collections.Generic;

    public interface IRenameRefactoring
    {
        string Namespace { get; }
        string OriginalInstanceName { get; }
        string InstanceName { get; }
    }

    public interface IAdditionalRenameRefactorings
    {
        List<string> AdditionalOriginalInstanceNames { get; }
        List<string> AdditionalInstanceNames { get; }
    }
}
