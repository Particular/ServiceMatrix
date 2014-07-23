namespace NServiceBusStudio
{
    partial class Command : IRenameRefactoring
    {
        public string Namespace
        {
            get { return Parent.Namespace; }
        }
    }
}
