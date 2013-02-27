using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Modeling;
using Microsoft.VisualStudio.Modeling.Diagrams;
using DslModeling = global::Microsoft.VisualStudio.Modeling;
using DslDiagrams = global::Microsoft.VisualStudio.Modeling.Diagrams;

namespace NServiceBus.Modeling.EndpointDesign
{
    /// <summary>
    /// Rule to call programmatically set the position of a Shape when
    /// it's added to the diagram (directly or indirectly). This rule
    /// gets called when the transaction's top level commit is called.
    /// </summary>
    [DslModeling::RuleOn(typeof(global::NServiceBus.Modeling.EndpointDesign.SendReceiveEndpoint), FireTime = DslModeling::TimeToFire.TopLevelCommit, Priority = DslDiagrams::DiagramFixupConstants.AutoLayoutShapesRulePriority + 1)]
    public class OrderShapeAddedToDiagramRule : AddRule
    {
        public static int SendReceiveEndpointCounter = 0;
        private const double margin = 0.25;

        public override void ElementAdded(ElementAddedEventArgs e)
        {
            SendReceiveEndpoint model = e.ModelElement as SendReceiveEndpoint;
            if (model == null || !PresentationViewsSubject.GetPresentation(model).Any())
                return;

            var leftElements = new List<ModelElement>().Concat(model.ProcessCommands).Concat(model.ProcessEvents).ToList();
            var rightElements = new List<ModelElement>().Concat(model.EmittedCommands).Concat(model.EmittedEvents).ToList();

            double maxElements = (leftElements.Count() > rightElements.Count()) ? leftElements.Count() : rightElements.Count();
            var maxHeight = maxElements * 2;

            OrderShapes(margin, maxHeight, leftElements);
            
            var shape = PresentationViewsSubject.GetPresentation(model).First() as SendReceiveEndpointShape;
            shape.IsExpanded = true;
            shape.Location = new PointD(2.5 + margin, (maxHeight / 2) - (shape.Size.Height / 2) + margin + SendReceiveEndpointCounter * 2);
            
            OrderShapes(5 + margin, maxHeight, rightElements);

            SendReceiveEndpointCounter++;
        }

        private void OrderShapes(double offsetX, double maxHeight, IList<ModelElement> modelElements)
        {
            var offset = maxHeight / (modelElements.Count() + 1);
            for (int i = 0; i < modelElements.Count(); i++)
            {
                var shape = PresentationViewsSubject.GetPresentation(modelElements[i]).First() as ImageShape;
                shape.IsExpanded = true;
                shape.Location = new PointD(offsetX, (offset * (i + 1) - (shape.Size.Height / 2) + margin + SendReceiveEndpointCounter * 2));
            }

            //y += shape.Size.Height + shape.Diagram.DefaultContainerMargin.Height;
        }
    }
}
