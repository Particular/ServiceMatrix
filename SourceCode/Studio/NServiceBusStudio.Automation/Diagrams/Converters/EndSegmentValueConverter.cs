using Mindscape.WpfDiagramming.Foundation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NServiceBusStudio.Automation.Diagrams.Converters
{
    public class EndSegmentValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            var segments = (ObservableCollection<DiagramConnectionSegment>)value;

            if (parameter.ToString() == "X")
            {
                return segments.Last().EndPoint.X;
            }
            else
            {
                return segments.Last().EndPoint.Y;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
