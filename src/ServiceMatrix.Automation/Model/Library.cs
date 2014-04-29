namespace NServiceBusStudio
{
    using System;
    partial class ServiceLibrary : ICodeLibrary, IRenameRefactoringNotSupported
    {
        public ICodeLibraries ParentLibraries
        {
            get { return Parent as ICodeLibraries; }
        }
    }

    partial class ServiceLibraries : ICodeLibraries
    {
        
    }

    partial class Library : ICodeLibrary, IRenameRefactoringNotSupported
    {
        public ICodeLibraries ParentLibraries
        {
            get { return Parent as ICodeLibraries; }
        }
    }

    partial class Libraries : ICodeLibraries
    {
        
    }

    public interface ICodeLibrary
    {
        string CodeIdentifier { get; }
        event EventHandler FilePathChanged;
        String FilePath { get; set; }
        String Notes { get; set; }
        String InstanceName { get; set; }
        ICodeLibraries ParentLibraries { get; }
    }

    public interface ICodeLibraries
    {
        string Namespace { get; set; }
    }
}
