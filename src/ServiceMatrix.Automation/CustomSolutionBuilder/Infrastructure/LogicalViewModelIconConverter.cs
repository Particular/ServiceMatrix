using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using NuPattern.Runtime.UI;
using NuPattern.Runtime;
using NServiceBusStudio.Automation.CustomSolutionBuilder.ViewModels;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Infrastructure
{
    using System.Globalization;

    public class LogicalViewModelIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var node = value as LogicalViewModel.LogicalViewModelNode;
            if (node != null)
            {
                if (string.IsNullOrEmpty(node.CustomIconPath))
                {
                    var model = node.InnerViewModel;
                    if (model != null)
                    {
                        var element = model.Data.Info as IPatternElementSchema;
                        if (element != null)
                        {
                            if (!string.IsNullOrEmpty(element.Icon))
                            {
                                return element.Icon;
                            }
                        }

                        return model.IconPath;
                    }
                }
                return node.CustomIconPath;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
