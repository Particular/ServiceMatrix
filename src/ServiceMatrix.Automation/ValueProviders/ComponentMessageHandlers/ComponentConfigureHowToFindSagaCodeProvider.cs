﻿namespace NServiceBusStudio.Automation.ValueProviders.ComponentMessageHandlers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;

    [CLSCompliant(false)]
    [Category("General")]
    [Description("Generate ConfigureHowToFindSagaBody value and Set Code properties.")]
    [DisplayName("Returns ConfigureHowToFindSagaBody value")]
    public class ComponentConfigureHowToFindSagaCodeProvider : ComponentFromLinkBasedValueProvider
    {
        public override object Evaluate()
        {
            return GenerateConfigureHowToFindSagaBody();
        }

        private string GenerateConfigureHowToFindSagaBody()
        {
            var sb = new StringBuilder();
            sb.AppendLine();

            var sagaStarters = new List<string>();
            sagaStarters.AddRange(Component.Subscribes.ProcessedCommandLinks.Where(x => x.StartsSaga).Select(x => x.CommandReference.Value.InstanceName));
            sagaStarters.AddRange(Component.Subscribes.SubscribedEventLinks.Where(x => x.StartsSaga).Select(x => x.EventReference.Value.InstanceName));
            sagaStarters.AddRange(Component.Subscribes.HandledMessageLinks.Where(x => x.StartsSaga).Select(x => x.MessageReference.Value.InstanceName));

            foreach (var cl in Component.Subscribes.ProcessedCommandLinks)
            {
                if (sagaStarters.Any(x => x != cl.CommandReference.Value.InstanceName))
                {
                    sb.AppendLine("            ConfigureMapping<" + cl.CommandReference.Value.InstanceName + ">(m => /* specify message property */).ToSaga(s =>  /* specify saga property */ );");
                }
            }

            foreach (var el in Component.Subscribes.SubscribedEventLinks)
            {
                if (sagaStarters.Any(x => x != el.EventReference.Value.InstanceName))
                {
                    sb.AppendLine("            ConfigureMapping<" + el.EventReference.Value.InstanceName + ">(m => /* specify message property */).ToSaga(s =>  /* specify saga property */ );");
                }
            }

            foreach (var hl in Component.Subscribes.HandledMessageLinks)
            {
                if (sagaStarters.Any(x => x != hl.MessageReference.Value.InstanceName))
                {
                    sb.AppendLine("            // ConfigureMapping<" + hl.MessageReference.Value.InstanceName + ">(m => /* specify message property */).ToSaga(s =>  /* specify saga property */ );");
                }
            }

            return sb.ToString();
        }
    }
}
