using System;
using System.Globalization;
using Xamarin.Forms;

namespace GoBabyGoV2.Utilities
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool) value
                ? Application.Current.Resources["ShieldColorActive"]
                : Application.Current.Resources["ShieldColorInActive"];
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}