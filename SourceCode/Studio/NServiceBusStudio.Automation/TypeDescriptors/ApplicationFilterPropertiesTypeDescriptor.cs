using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Microsoft.VisualStudio.Patterning.Runtime;
using Microsoft.VisualStudio.Patterning.Extensibility;

namespace NServiceBusStudio.Automation.TypeDescriptors
{
    public class ApplicationFilterPropertiesTypeDescriptor: CustomTypeDescriptor
    {
        private ICustomTypeDescriptor parent;
        private IProduct instance;

        public ApplicationFilterPropertiesTypeDescriptor(ICustomTypeDescriptor parent, IProduct product)
        {
            this.parent = parent;
            this.instance = product;
        }

        // TODO: Modify this to implement an interface like the following one:
        // ToolkitExtensions
        //    .For<IApplication>()
        //    .When(a => a.SqlServer == "")
        //    .Properties(() => a.Deploy)
        //    .Attributes(() => new BrowsableAttribute(false));
        // Take advantage of new DelegatingPropertyDescriptor(x, attrs)

        public override PropertyDescriptorCollection GetProperties()
        {
            var list = parent.GetProperties().OfType<PropertyDescriptor>().ToList();

            // If Application element
            if (this.instance.DefinitionId == new Guid("2c52bbfe-442d-4f40-8f6f-7df75dd99cac"))
            {
                var transportProperty = list.First(x => x.Name == "Transport");
                var tranport = transportProperty.GetValue(instance).ToString();

                // If Transport != SqlServer
                if (tranport != TransportType.SqlServer.ToString())
                {
                    // Filter List
                    return new PropertyDescriptorCollection(list.Where(x => x.Name != "SqlServer" && x.Name != "SqlDatabase").ToArray());
                }
            }

            return new PropertyDescriptorCollection(list.ToArray());
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.GetProperties();
        }
    }
}
