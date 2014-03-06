using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core.Design;
using NServiceBusStudio.Core;
using NuPattern.ComponentModel;


namespace NServiceBusStudio
{
    public class HandledMessageReferenceConverter : ElementReferenceConverter<IHandledMessageLink, IMessage, HandledMessageReferenceStrategy> { }

    public class HandledMessageReferenceEditor : ElementReferenceEditor<IHandledMessageLink, IMessage, HandledMessageReferenceStrategy> { }

    public class HandledMessageReferenceStrategy : IElementReferenceStrategy<IHandledMessageLink, IMessage>
    {
        public IElementReference<IMessage> GetReference(IHandledMessageLink owner)
        {
            return owner.MessageReference;
        }

        public IEnumerable<StandardValue> GetStandardValues(IHandledMessageLink owner)
        {
            var thisService = owner.Parent.Parent;

            return owner.Parent.Parent.Parent.Parent.Contract.Messages.Message
                .Except(owner.Parent.HandledMessageLinks.Select(link => link.MessageReference.Value)
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

            public IEnumerable<NuPattern.Runtime.IReference> References
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

            public NuPattern.Runtime.IElement AsElement()
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
