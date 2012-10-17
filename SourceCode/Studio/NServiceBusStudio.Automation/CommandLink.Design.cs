using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBusStudio.Core.Design;
using NServiceBusStudio.Core;
using Microsoft.VisualStudio.TeamArchitect.PowerTools.Features.Design;

namespace NServiceBusStudio
{
    public class CommandReferenceConverter : ElementReferenceConverter<ICommandLink, ICommand, CommandReferenceStrategy> { }

    public class CommandReferenceEditor : ElementReferenceEditor<ICommandLink, ICommand, CommandReferenceStrategy> { }

    public class CommandReferenceStrategy : IElementReferenceStrategy<ICommandLink, ICommand>
    {
        public IElementReference<ICommand> GetReference(ICommandLink owner)
        {
            return owner.CommandReference;
        }

        public IEnumerable<StandardValue> GetStandardValues(ICommandLink owner)
        {
            var thisService = owner.Parent.Parent;

            return owner.Parent.Parent.Parent.Parent.Contract.Commands.Command
                .Except(owner.Parent.CommandLinks.Select(link => link.CommandReference.Value)
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

            public IEnumerable<Microsoft.VisualStudio.Patterning.Runtime.IReference> References { get; private set; }

            public string Notes { get; set; }

            public bool InTransaction { get; private set; }

            public bool IsSerializing { get; private set; }

            public ICommands Parent
            {
                get { throw new NotImplementedException(); }
            }

            public void Delete()
            {
                throw new NotImplementedException();
            }

            public Microsoft.VisualStudio.Patterning.Runtime.IElement AsElement()
            {
                throw new NotImplementedException();
            }

            public string CodeIdentifier { get; private set; }

            public TRuntimeInterface As<TRuntimeInterface>() where TRuntimeInterface : class
            {
                throw new NotImplementedException();
            }

            public event EventHandler InstanceNameChanged;


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
