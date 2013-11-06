using Mindscape.WpfDiagramming;
using Newtonsoft.Json;
using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
using ServiceMatrix.Diagramming.ViewModels.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace ServiceMatrix.Diagramming.ViewModels
{
    public class ServiceMatrixDiagramLayoutAlgorithm
    {
        public ServiceMatrixDiagramMindscapeViewModel ViewModel { get; set; }
        public string FilePath { get; set; }
        public Dictionary<Guid, Point> ShapePositions { get; set; }

        private const double Endpoint_LeftAlignment = 50;
        private const double Endpoint_RightAlignment = 950;
        private const double Message_Alignment = 500;

        public ServiceMatrixDiagramLayoutAlgorithm(ServiceMatrixDiagramMindscapeViewModel viewModel)
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

        public void RemoveElementPosition(GroupableNode node)
        {
            this.SaveElementPosition(node, null);
        }

        private Point CalculateElementPosition(GroupableNode node)
        {
            
            var x = 50.0;
            var y = 100.0;

            if (node is EndpointNode)
            {
                var leftAlignment = GetYPosition(Endpoint_LeftAlignment);
                var rightAlignment = GetYPosition(Endpoint_RightAlignment);

                if (leftAlignment <= rightAlignment)
                {
                    x = Endpoint_LeftAlignment;
                }
                else
                {
                    x = Endpoint_RightAlignment;
                }
            }
            else if (node is CommandNode || node is EventNode)
            {
                x = Message_Alignment;
            }

            y = GetYPosition(x);

            return new Point(x, y);
        }

        private double GetYPosition(double x)
        {
            const int ShapeWidth = 350;
            double y = 100.0;

            var shapesOnSimilarXPosition = this.ViewModel.Nodes.Cast<GroupableNode>().Where(n => n.ParentNode == null &&
                                                                                            ((n.Bounds.X >= x && n.Bounds.X <= x + ShapeWidth) ||
                                                                                            (n.Bounds.X + n.Bounds.Width >= x && n.Bounds.X + n.Bounds.Width <= x + ShapeWidth)));
            if (shapesOnSimilarXPosition.Any())
            {
                y = shapesOnSimilarXPosition.Max(n => n.Bounds.Y + n.Bounds.Height);
                y = y + 50;
            }
            return y;
        }

        public Point? LoadElementPosition(GroupableNode node)
        {
            if (ShapePositions.Any(x => x.Key == node.Id))
            {
                return ShapePositions.First(x => x.Key == node.Id).Value;
            }

            return null;
        }

        public void SaveElementPosition(GroupableNode node, Point? point)
        {
            if (ShapePositions.Any(x => x.Key == node.Id))
            {
                ShapePositions.Remove(node.Id);
            }

            if (point.HasValue)
            {
                ShapePositions.Add(node.Id, point.Value);
            }

            // Saving into file
            if (this.FilePath != null)
            {
                var fileContent = JsonConvert.SerializeObject(this.ShapePositions);
                File.WriteAllText(this.FilePath, fileContent);
            }
        }


    }

}
