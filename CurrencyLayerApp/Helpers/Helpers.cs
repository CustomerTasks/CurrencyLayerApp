using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Helpers
{
    static class Helpers
    {
        public static Currency ToCurrency(this CurrencyModel model)=> new  Currency {Code = model.Code,Name = model.Name,Rating = model.Rating};
        public static CurrencyModel ToCurrencyModel(this Currency model) => new CurrencyModel { Code = model.Code, Name = model.Name, Rating = model.Rating};

        public static ExchangeModel ToExchangeModel(this CurrencyModel model, double multiplyNumber) => new ExchangeModel()
        {
            Code = model.Code,
            Value = model.Rating * multiplyNumber
        };

        internal static ImageSource GetImage(string str,string format)
        {
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource =
                new Uri($"{CommonData.IconFolder}{str}.{format}",
                    UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            return bimage;
        }

        public static ImageSource GetImage(ICurrency currency)
        {
           return GetImage(new string(currency.Code.ToLower().Take(2).ToArray()),"png");
        }
    }
}
