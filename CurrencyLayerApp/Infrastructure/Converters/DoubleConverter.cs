using System;
using System.Globalization;
using System.Windows.Data;

namespace CurrencyLayerApp.Infrastructure.Converters
{
    [ValueConversion(typeof(string), typeof(double))]
    class DoubleConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value is double)) return value;
            return ((double) value).ToString(CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException();
        }
    }
}
