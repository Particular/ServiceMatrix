namespace NServiceBusStudio.Automation.Exceptions
{
    using System;
    public class ElementAlreadyExistsException : OperationCanceledException
    {
        public string ElementType { get; set; }
        public string ElementName { get; set; }

        public ElementAlreadyExistsException(string message, string elementType, string elementName) : base (message)
        {
            ElementType = elementType;
            ElementName = elementName;
        }
    }
}
