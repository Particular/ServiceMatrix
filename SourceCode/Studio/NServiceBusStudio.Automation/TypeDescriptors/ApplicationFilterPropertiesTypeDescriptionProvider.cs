//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.ComponentModel;
//using NuPattern.Runtime.Store;
//using NuPattern.Runtime;
//
//namespace NServiceBusStudio.Automation.TypeDescriptors
//{
//    public class ApplicationFilterPropertiesTypeDescriptionProvider : TypeDescriptionProvider
//    {
//        private static readonly TypeDescriptionProvider parent = TypeDescriptor.GetProvider(typeof(Product));
//
//        public override ICustomTypeDescriptor GetTypeDescriptor(Type objectType, object instance)
//        {
//            return new ApplicationFilterPropertiesTypeDescriptor(parent.GetTypeDescriptor(objectType, instance), (IProduct)instance);
//        }
//    }
//}
