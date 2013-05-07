using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;
using Microsoft.VisualStudio.PlatformUI;
using NuPattern.Reflection;
using NuPattern.Runtime.UI.ViewModels;

namespace NuPattern.Runtime.UI.Data
{
    /// <summary>
    /// Applies grouping to the given <see cref="IEnumerable"/> collection of menus.
    /// </summary>
    [ValueConversion(typeof(IEnumerable), typeof(ICollectionView))]
    public class MenuGroupingEnumerableConverter : ValueConverter<IEnumerable, ICollectionView>
    {
        /// <summary>
        /// Converts the specified value.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="parameter">The parameter to use as property name in the grouping.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>A <see cref="ICollectionView"/> with grouping applied.</returns>
        protected override ICollectionView Convert(IEnumerable value, object parameter, CultureInfo culture)
        {
            var view = CollectionViewSource.GetDefaultView(value);

            // Add menu sorting
            view.SortDescriptions.Clear();
            view.SortDescriptions.Add(new SortDescription(Reflector<IMenuOptionViewModel>.GetPropertyName(mnu => mnu.GroupIndex), ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription(Reflector<IMenuOptionViewModel>.GetPropertyName(mnu => mnu.SortOrder), ListSortDirection.Ascending));
            view.SortDescriptions.Add(new SortDescription(Reflector<IMenuOptionViewModel>.GetPropertyName(mnu => mnu.Caption), ListSortDirection.Ascending));

            // Add menu grouping
            view.GroupDescriptions.Clear();
            view.GroupDescriptions.Add(new PropertyGroupDescription(Reflector<IMenuOptionViewModel>.GetPropertyName(mnu => mnu.GroupIndex)));

            return view;
        }
    }
}