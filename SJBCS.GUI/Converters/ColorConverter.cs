using System;
using System.Globalization;
using System.Windows.Data;

namespace SJBCS.GUI.Converters
{
    public class ColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string state = value.ToString();

            switch (state)
            {
                case "Error":
                    return "Red";

                case "Informational":
                    return "Blue";

                case "Warning":
                    return "Orange";

                case "Validation":
                    return "Orange";

                case "Success":
                    return "Green";

                case "Connected":
                    return "Green";

                case "Disconnected":
                    return "Red";

                case "logged in.":
                    return "Green";

                case "logged out.":
                    return "Red";

                default:
                    return "Blue";
            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
