using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CurrencyLayerApp.Helpers;
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
    [ValueConversion(typeof(string), typeof(CurrencyModel))]
    class CurrentModelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var model = (CurrencyModel)value;
            return model.Code;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return new CurrencyModel(){Code = value.ToString(), IsSelected = true};
        }
    }
    
}
