namespace ServiceMatrix.Diagramming.ViewModels
{
    using Newtonsoft.Json;
    using NuPattern.Diagnostics;
    using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
    using ServiceMatrix.Diagramming.ViewModels.Shapes;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows;

    public class ServiceMatrixDiagramLayoutAlgorithm
    {
        private static readonly ITracer tracer = Tracer.Get<ServiceMatrixDiagramLayoutAlgorithm>();

        public ServiceMatrixDiagramMindscapeViewModel ViewModel { get; set; }
        public string FilePath { get; set; }
        public Dictionary<Guid, Point> ShapePositions { get; set; }

        private const double Endpoint_LeftAlignment = 50;
        private const double Endpoint_RightAlignment = 950;
        private const double Message_Alignment = 500;

        public ServiceMatrixDiagramLayoutAlgorithm(ServiceMatrixDiagramMindscapeViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public void LoadShapePositions (string solutionFolder)
        {
            FilePath = Path.Combine(solutionFolder, "DiagramShapePositions.json");

            // Load shape positions from file
            try
            {
                if (File.Exists(FilePath))
                {
                    var fileContent = File.ReadAllText(FilePath);
                    ShapePositions = JsonConvert.DeserializeObject<Dictionary<Guid, Point>>(fileContent);
                }
            }
            catch (Exception ex)
            {
                tracer.Error(ex, "Cannot load shape positions from DiagramShapePositions.json.");
            }

            // If File not exists or an error ocurred
            if (ShapePositions == null)
            {
                ShapePositions = new Dictionary<Guid, Point>();
            }
        }

        public void UnloadShapePositiions ()
        {
            FilePath = null;
            ShapePositions = null;
        }


        public void SetElementPosition(GroupableNode node)
        {
            var position = LoadElementPosition(node);

            if (!position.HasValue)
            {
                position = CalculateElementPosition(node);
                SaveElementPosition(node, position.Value);
            }

            node.BoundsChanged += (s, e) => SaveElementPosition(node, node.Bounds.Location);
            node.Bounds = new Rect (position.Value, node.Bounds.Size);
        }

        public void RemoveElementPosition(GroupableNode node)
        {
            SaveElementPosition(node, null);
        }

        private Point CalculateElementPosition(GroupableNode node)
        {
            
            var x = 50.0;
            double y;

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
            else if (node is CommandNode || node is EventNode || node is MessageNode)
            {
                x = Message_Alignment;
            }

            y = GetYPosition(x);

            return new Point(x, y);
        }

        private double GetYPosition(double x)
        {
            const int ShapeWidth = 350;
            var y = 100.0;

            var shapesOnSimilarXPosition = ViewModel.Nodes.Cast<GroupableNode>().Where(n => n.ParentNode == null &&
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
            if (FilePath != null)
            {
                try
                {
                    var fileContent = JsonConvert.SerializeObject(ShapePositions);
                    File.WriteAllText(FilePath, fileContent);
                }
                catch (Exception ex) 
                {
                    tracer.Error(ex, "Cannot save shape positions to DiagramShapePositions.json.");
                }
            }
        }


    }

}
