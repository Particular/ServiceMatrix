namespace NServiceBusStudio
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NServiceBusStudio.Core.Design;
    using NServiceBusStudio.Core;
    using NuPattern.ComponentModel;

    using NuPattern.Runtime;

    public class ProcessedCommandReplyReferenceConverter : ElementReferenceConverter<IProcessedCommandLinkReply, IMessage, ProcessedCommandReplyReferenceStrategy> { }

    public class ProcessedCommandReplyReferenceEditor : ElementReferenceEditor<IProcessedCommandLinkReply, IMessage, ProcessedCommandReplyReferenceStrategy> { }

    public class ProcessedCommandReplyReferenceStrategy : IElementReferenceStrategy<IProcessedCommandLinkReply, IMessage>
    {
        public IElementReference<IMessage> GetReference(IProcessedCommandLinkReply owner)
        {
            return owner.MessageReference;
        }

        public IEnumerable<StandardValue> GetStandardValues(IProcessedCommandLinkReply owner)
        {
            return owner.Parent.Parent.Parent.Parent.Parent.Contract.Messages.Message
                .Except(owner.Parent.Parent.ProcessedCommandLinks.Select(link => link.ProcessedCommandLinkReply.MessageReference.Value)
                    .Except(new[] { owner.MessageReference.Value }))
                .Select(e => new StandardValue(e.InstanceName, e));
        }

        public IMessage NullValue
        {
            get { return NullMessage.Instance; }
        }

        /// <summary>
        /// Implements the null pattern for components. 
        /// </summary>
        /// <devdoc>
        /// Need this to workaround the behavior of the FB StandardValuesEditor 
        /// that considers a null value as equal to selecting the original 
        /// value.
        /// </devdoc>
        private class NullMessage : IMessage
        {
            static NullMessage()
            {
                Instance = new NullMessage();
            }

            private NullMessage()
            {
            }

            public static IMessage Instance { get; private set; }

            // ------------------------------------------------- // 



            public string InstanceName
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public double InstanceOrder
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public IEnumerable<IReference> References
            {
                get { throw new NotImplementedException(); }
            }

            public string Notes
            {
                get
                {
                    throw new NotImplementedException();
                }
                set
                {
                    throw new NotImplementedException();
                }
            }

            public bool InTransaction
            {
                get { throw new NotImplementedException(); }
            }

            public bool IsSerializing
            {
                get { throw new NotImplementedException(); }
            }

            public IMessages Parent
            {
                get { throw new NotImplementedException(); }
            }

            public void Delete()
            {
                throw new NotImplementedException();
            }

            public IElement AsElement()
            {
                throw new NotImplementedException();
            }

            public string CodeIdentifier
            {
                get { throw new NotImplementedException(); }
            }

            public TRuntimeInterface As<TRuntimeInterface>() where TRuntimeInterface : class
            {
                throw new NotImplementedException();
            }

            public event EventHandler InstanceNameChanged;
        }
    }
}
