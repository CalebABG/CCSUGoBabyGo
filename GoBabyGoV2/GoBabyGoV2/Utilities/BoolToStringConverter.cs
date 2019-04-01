using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace GoBabyGoV2.Utilities
{
    public class BoolToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return "null";

            return (bool)value ? "Calc" : "Raw";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null)
                return false;

            var val = value as string;

            return val == "Raw";
        }
    }
}
