using System;
using System.Globalization;
using System.Windows.Data;

namespace SJBCS.GUI.Converters
{
    public class TimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime dateValue;

            if (value == null)
                return null;

            if (DateTime.TryParse(value.ToString(), out dateValue))
                return dateValue.ToString("hh:mm tt");

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return string.Empty;

            return value.ToString();
        }
    }
}
