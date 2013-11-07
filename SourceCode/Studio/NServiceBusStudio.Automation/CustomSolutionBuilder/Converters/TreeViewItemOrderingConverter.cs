using System;
using System.Collections;
using System.ComponentModel;
using System.Windows.Data;

namespace NServiceBusStudio.Automation.CustomSolutionBuilder.Converters
{
    /// <summary>
    /// Returns the sort descriptions for the treeview.
    /// </summary>
    public class TreeViewItemOrderingConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            IList collection = value as System.Collections.IList;
            ListCollectionView view = new ListCollectionView(collection);
            SortDescription sort = new SortDescription(parameter.ToString(), ListSortDirection.Ascending);
            view.SortDescriptions.Add(sort);

            return view;
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
