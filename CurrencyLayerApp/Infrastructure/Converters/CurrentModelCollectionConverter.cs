using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.Converters
{
    [ValueConversion(typeof(ObservableCollection<string>),typeof(ObservableCollection<CurrencyModel>))]
    class CurrentModelCollectionConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var model = (ObservableCollection<CurrencyModel>) value;
            return new ObservableCollection<string>(model.Select(x=>x.Code));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}