﻿using Mindscape.WpfDiagramming;
using ServiceMatrix.Diagramming.ViewModels.BaseViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceMatrix.Diagramming.ViewModels.Connections
{
    public abstract class BaseConnection : DiagramConnection
    {
        private bool _isHighlighted = false;
        private bool _isShadowed = false;

        // This is for highlighting the node if mouse is over.
        public bool IsHighlighted
        {
            get { return _isHighlighted; }
            set
            {
                Set<bool>(ref _isHighlighted, value, "IsHighlighted");
            }
        }

        // This is for highlighting the node if mouse is over.
        public bool IsShadowed
        {
            get { return _isShadowed; }
            set
            {
                Set<bool>(ref _isShadowed, value, "IsShadowed");
            }
        }

        private DiagramNode _source = null;
        public DiagramNode Source 
        {
            get { return _source; } 
            set 
            {
                if (_source != null)
                {
                    foreach (var connectionPoint in _source.ConnectionPoints)
	                {
                        connectionPoint.PositionChanged -= RecalculateConnectionPosition;
	                }
                }

                _source = value;
                foreach (var connectionPoint in _source.ConnectionPoints)
                {
                    connectionPoint.PositionChanged += RecalculateConnectionPosition;
                }
            } 
        }

        private DiagramNode _target = null;
        public DiagramNode Target
        {
            get { return _target; }
            set
            {
                if (_target != null)
                {
                    foreach (var connectionPoint in _target.ConnectionPoints)
                    {
                        connectionPoint.PositionChanged -= RecalculateConnectionPosition;
                    }
                }

                _target = value;
                foreach (var connectionPoint in _target.ConnectionPoints)
                {
                    connectionPoint.PositionChanged += RecalculateConnectionPosition;
                }
            }
        }

        public BaseConnection(DiagramNode from, DiagramNode to)
            : base(null, null)
        {
            this.Source = from;
            this.Target = to;
            this.LineType = null;
            this.ZOrder = ++GroupableNode.ZOrderCounter;

            RecalculateConnectionPosition(null, null);
        }


        void RecalculateConnectionPosition(object sender, EventArgs e)
        {
            var distances = new List<Tuple<double, DiagramConnectionPoint, DiagramConnectionPoint>>();


            foreach (var sourceConnectionPoint in this.Source.ConnectionPoints)
            {
                foreach (var targetConnectionPoint in this.Target.ConnectionPoints)
                {
                    distances.Add(new Tuple<double, DiagramConnectionPoint, DiagramConnectionPoint>(
                        PointDistance(sourceConnectionPoint.Position, targetConnectionPoint.Position),
                        sourceConnectionPoint,
                        targetConnectionPoint));
                }
            }


            var minDistance = distances.OrderBy(x => x.Item1).First();

            if (this.FromConnectionPoint != minDistance.Item2)
            {
                this.FromConnectionPoint = minDistance.Item2;
            }

            if (this.ToConnectionPoint != minDistance.Item3)
            {
                this.ToConnectionPoint = minDistance.Item3;
            }
            
        }

        private double PointDistance(Point start, Point end)
        {
            return Math.Sqrt(Math.Pow(end.X - start.X, 2) + Math.Pow(end.Y - start.Y, 2));
        }
    }
}
