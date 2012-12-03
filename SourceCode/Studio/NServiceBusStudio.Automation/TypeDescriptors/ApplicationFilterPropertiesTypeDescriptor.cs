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

                ShowHideProperty(ref list, "TransportBrokerUri", tranport == TransportType.ActiveMQ.ToString());
                ShowHideProperty(ref list, "TransportSqlServer", tranport == TransportType.SqlServer.ToString());
                ShowHideProperty(ref list, "TransportSqlDatabase", tranport == TransportType.SqlServer.ToString());

                // Filter List
                return new PropertyDescriptorCollection(list.ToArray());
            }

            return new PropertyDescriptorCollection(list.ToArray());
        }

        private void ShowHideProperty(ref List<PropertyDescriptor> list, string propertyName, bool isVisible)
        {
            if (!isVisible)
            {
                list = list.Where(x => x.Name != propertyName).ToList();
            }
        }

        public override PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            return this.GetProperties();
        }
    }
}
