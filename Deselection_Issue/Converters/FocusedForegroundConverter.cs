using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Deselection_Issue.Converters
{
    public class FocusedForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool isFocused && isFocused) ? Brushes.Green : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}