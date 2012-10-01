//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace NServiceBusStudio
//{
//    partial class ServiceLibrary : ICodeLibrary
//    {
//        public ICodeLibraries ParentLibraries 
//        {
//            get { return this.Parent as ICodeLibraries; }
//        }
//    }

//    partial class ServiceLibraries : ICodeLibraries
//    {
//    }

//    partial class Library : ICodeLibrary
//    {
//        public ICodeLibraries ParentLibraries
//        {
//            get { return this.Parent as ICodeLibraries; }
//        }
//    }

//    partial class Libraries : ICodeLibraries
//    {
//    }

//    public interface ICodeLibrary
//    {
//        string CodeIdentifier { get; }
//        event EventHandler FilePathChanged;
//        String FilePath { get; set; }
//        String Notes { get; set; }
//        String InstanceName { get; set; }
//        ICodeLibraries ParentLibraries { get; }
//    }

//    public interface ICodeLibraries
//    {
//        string Namespace { get; set; }
//    }
//}
