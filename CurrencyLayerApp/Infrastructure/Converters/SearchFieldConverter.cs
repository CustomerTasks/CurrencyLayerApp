using System;
using System.Globalization;
using System.Windows.Data;

namespace CurrencyLayerApp.Infrastructure.Converters
{
    [ValueConversion(typeof(string), typeof(string))]
    class SearchFieldConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is string)) return null;

            return ((string) value).ToLower();
        }
    }
}
