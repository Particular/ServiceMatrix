using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NServiceBusStudio.Automation.Exceptions
{
    public class ElementAlreadyExistsException : OperationCanceledException
    {
        public string ElementType { get; set; }
        public string ElementName { get; set; }

        public ElementAlreadyExistsException(string message, string elementType, string elementName) : base (message)
        {
            this.ElementType = elementType;
            this.ElementName = elementName;
        }
    }
}
