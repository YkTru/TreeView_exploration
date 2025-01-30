using System;
using System.Globalization;
using System.Windows.Data;
using Deselection_Issue.ViewModels;

namespace Deselection_Issue.Converters
{
    public class SelectedItemToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is HierarchicalItemViewModel item ? item.Name : "No item selected";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}