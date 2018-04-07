using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SJBCS.GUI.Converters
{
    public class NegatableStringToVisibilityConverter : IValueConverter
    {
        public bool Negate { get; set; }
        public Visibility FalseVisibility { get; set; }
        public NegatableStringToVisibilityConverter()
        {
            FalseVisibility = Visibility.Collapsed;
        }
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string sVal = value.ToString();
            
            if(sVal == "Success" || sVal == "Information" || sVal == "Error" || sVal == "Warning")
            {
                sVal = "False";
            }
            else
            {
                sVal = "True";
            }

            bool bVal;
            bool result = bool.TryParse(sVal.ToString(), out bVal);
            if (!result) return value;
            if (bVal && !Negate) return Visibility.Visible;
            if (bVal && Negate) return FalseVisibility;
            if (!bVal && Negate) return Visibility.Visible;
            if (!bVal && !Negate) return FalseVisibility;
            else return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
