using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace SJBCS.GUI.Converters
{
    public class PasswordBoxConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Implement type and value check here...
            return new PasswordBoxWrapper((PasswordBox)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException("No conversion.");
        }
    }
}
