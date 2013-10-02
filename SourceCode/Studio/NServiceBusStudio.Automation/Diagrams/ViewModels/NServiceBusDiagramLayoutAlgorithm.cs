using Mindscape.WpfDiagramming;
using Newtonsoft.Json;
using NServiceBusStudio.Automation.Diagrams.ViewModels.BaseViewModels;
using NServiceBusStudio.Automation.Diagrams.ViewModels.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace NServiceBusStudio.Automation.Diagrams.ViewModels
{
    public class NServiceBusDiagramLayoutAlgorithm
    {
        public NServiceBusDiagramMindscapeViewModel ViewModel { get; set; }
        public string FilePath { get; set; }
        public Dictionary<Guid, Point> ShapePositions { get; set; }

        public NServiceBusDiagramLayoutAlgorithm(NServiceBusDiagramMindscapeViewModel viewModel)
        {
            this.ViewModel = viewModel;
        }

        public void LoadShapePositions (string solutionFolder)
        {
            this.FilePath = Path.Combine(solutionFolder, "DiagramShapePositions.json");

            // Load shape positions from file
            try
            {
                if (File.Exists(this.FilePath))
                {
                    var fileContent = File.ReadAllText(this.FilePath);
                    this.ShapePositions = JsonConvert.DeserializeObject<Dictionary<Guid, Point>>(fileContent);
                }
            }
            catch { }

            // If File not exists or an error ocurred
            if (this.ShapePositions == null)
            {
                this.ShapePositions = new Dictionary<Guid, Point>();
            }
        }

        public void UnloadShapePositiions ()
        {
            this.FilePath = null;
            this.ShapePositions = null;
        }


        public void SetElementPosition(GroupableNode node)
        {
            var position = this.LoadElementPosition(node);

            if (!position.HasValue)
            {
                position = this.CalculateElementPosition(node);
                this.SaveElementPosition(node, position.Value);
            }

            node.BoundsChanged += (s, e) => this.SaveElementPosition(node, node.Bounds.Location);
            node.Bounds = new Rect (position.Value, node.Bounds.Size);
        }

        private Point CalculateElementPosition(GroupableNode node)
        {
            const int ShapeWidth = 350;
            var x = 50.0;
            var y = 100.0;

            if (node is EndpointNode)
            {
                // Align endpoints algorithm
                var endpointsCount = this.ViewModel.Nodes.Count(n => n is EndpointNode);
                // Snap to 2 columns
                if (endpointsCount % 2 == 0)
                {
                    x = 50;
                }
                else
                {
                    x = 950;
                }
            }
            else if (node is CommandNode || node is EventNode)
            {
                x = 500;
            }

            var shapesOnSimilarXPosition = this.ViewModel.Nodes.Cast<GroupableNode>().Where(n => n.ParentNode == null &&
                                                                                                 ((n.Bounds.X >= x && n.Bounds.X <= x + ShapeWidth) ||
                                                                                                  (n.Bounds.X + n.Bounds.Width >= x && n.Bounds.X + n.Bounds.Width <= x + ShapeWidth)));
            if (shapesOnSimilarXPosition.Any())
            {
                y = shapesOnSimilarXPosition.Max(n => n.Bounds.Y + n.Bounds.Height);
                y = y + 100;
            }

            return new Point(x, y);
        }

        public Point? LoadElementPosition(GroupableNode node)
        {
            if (ShapePositions.Any(x => x.Key == node.Id))
            {
                return ShapePositions.First(x => x.Key == node.Id).Value;
            }

            return null;
        }

        public void SaveElementPosition(GroupableNode node, Point point)
        {
            if (ShapePositions.Any(x => x.Key == node.Id))
            {
                ShapePositions.Remove(node.Id);
            }

            ShapePositions.Add(node.Id, point);

            // Saving into file
            if (this.FilePath != null)
            {
                var fileContent = JsonConvert.SerializeObject(this.ShapePositions);
                File.WriteAllText(this.FilePath, fileContent);
            }
        }


    }

}
