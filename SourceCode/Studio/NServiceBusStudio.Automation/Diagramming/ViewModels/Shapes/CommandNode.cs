using Mindscape.WpfDiagramming;
using NuPattern.Runtime.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ServiceMatrix.Diagramming.ViewModels.Shapes
{
    public class CommandNode : MessageBaseNode
    {
        public CommandNode(IProductElementViewModel innerViewModel)
            : base(innerViewModel)
        {
        }
    }
}
