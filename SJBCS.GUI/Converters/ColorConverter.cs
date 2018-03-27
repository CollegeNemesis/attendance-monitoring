using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

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

                case "InformationB":
                    return "Blue";

                case "Warning":
                    return "Orange";

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
