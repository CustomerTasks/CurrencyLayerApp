using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using CurrencyLayerApp.Infrastructure.Global;

namespace CurrencyLayerApp.Infrastructure.Converters
{
    [ValueConversion(typeof(SolidColorBrush),typeof(Logger.Color))]
    class LogColorConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return value;
            Brush brush = Brushes.LightGray;
            switch ((Logger.Color) value)
            {
                case Logger.Color.Gray:
                    brush = Brushes.LightGray;
                    break;
                case Logger.Color.Green:
                    brush = Brushes.LightSeaGreen;
                    break;
                case Logger.Color.Red:
                    brush = Brushes.DarkRed;
                    break;
            }
            return brush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
