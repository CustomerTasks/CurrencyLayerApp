﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.Converters
{
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
