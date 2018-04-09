using System;
using System.Globalization;
using System.Windows.Data;

namespace SJBCS.GUI.Converters
{
    public class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "InformationOutline";

            string state = value.ToString();

            switch (state)
            {
                case "Error":
                    return "MessageAlert";

                case "Informational":
                    return "Information";

                case "Warning":
                    return "CommentAlert";

                case "Validation":
                    return "CommentQuestionOutline";

                case "Success":
                case "Connected":
                    return "CheckCircle";

                case "Disconnected":
                case "Fingerprint not recognized.":
                case "Failed to process fingerprint, try again.":
                    return "CloseCircle";

                case "logged in.":
                    return "Login";

                case "logged out.":
                    return "Logout";

                case "Collapsed":
                    return "ChevronRight";

                case "VisibleWhenSelected":
                    return "ChevronDown";

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
