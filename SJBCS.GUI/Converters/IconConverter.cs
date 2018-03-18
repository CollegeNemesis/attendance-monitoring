using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace SJBCS.GUI.Converters
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string state = (string)value;
            switch (state)
            {
                case "Connected":
                    return "CheckCircle";

                case "Disconnected":
                    return "CloseCircle";

                case "Fingerprint not recognized.":
                    return "CloseCircle";

                case "logged in.":
                    return "Login";

                case "logged out.":
                    return "Logout";

                default:
                    return "InformationOutline";
            }



        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
