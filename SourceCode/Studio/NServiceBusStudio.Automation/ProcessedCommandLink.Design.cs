using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core.Design;
using NServiceBusStudio.Core;
using NuPattern.ComponentModel;


namespace NServiceBusStudio
{
    public class ProcessedCommandReferenceConverter : ElementReferenceConverter<IProcessedCommandLink, ICommand, ProcessedCommandReferenceStrategy> { }

    public class ProcessedCommandReferenceEditor : ElementReferenceEditor<IProcessedCommandLink, ICommand, ProcessedCommandReferenceStrategy> { }

    public class ProcessedCommandReferenceStrategy : IElementReferenceStrategy<IProcessedCommandLink, ICommand>
    {
        public IElementReference<ICommand> GetReference(IProcessedCommandLink owner)
        {
            return owner.CommandReference;
        }

        public IEnumerable<StandardValue> GetStandardValues(IProcessedCommandLink owner)
        {
            var thisService = owner.Parent.Parent;

            return owner.Parent.Parent.Parent.Parent.Contract.Commands.Command
                .Except(owner.Parent.ProcessedCommandLinks.Select(link => link.CommandReference.Value)
                    .Except(new[] { owner.CommandReference.Value }))
                .Select(e => new StandardValue(e.InstanceName, e));
        }

        public ICommand NullValue
        {
            get { return NullCommand.Instance; }
        }

        /// <summary>
        /// Implements the null pattern for components. 
        /// </summary>
        /// <devdoc>
        /// Need this to workaround the behavior of the FB StandardValuesEditor 
        /// that considers a null value as equal to selecting the original 
        /// value.
        /// </devdoc>
        private class NullCommand : ICommand
        {
            static NullCommand()
            {
                Instance = new NullCommand();
            }

            private NullCommand()
            {
            }

            public static ICommand Instance { get; private set; }

            // ------------------------------------------------- // 

            public string InstanceName { get; set; }

            public IEnumerable<NuPattern.Runtime.IReference> References { get; private set; }

            public string Notes { get; set; }

            public bool InTransaction { get; private set; }

            public bool IsSerializing { get; private set; }

            public IEvents Parent
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

            public string CodeIdentifier { get; private set; }

            public TRuntimeInterface As<TRuntimeInterface>() where TRuntimeInterface : class
            {
                throw new NotImplementedException();
            }

            public event EventHandler InstanceNameChanged;

            ICommands ICommand.Parent
            {
                get { throw new NotImplementedException(); }
            }


            public double InstanceOrder
            {
                get
                {
                    return 0;
                }
                set
                {
                }
            }

            public bool DoNotAutogenerateComponents { get; set; }


            public event EventHandler DoNotAutogenerateComponentsChanged;
        }
    }
}
