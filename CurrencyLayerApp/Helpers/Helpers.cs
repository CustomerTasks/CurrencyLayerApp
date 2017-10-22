using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Helpers
{
    static class Helpers
    {
        /// <summary>
        /// Converts DB Entity into App Model
        /// </summary>
        /// <param name="model">DB Entity</param>
        /// <returns>App model</returns>
        public static Currency ToCurrency(this CurrencyModel model) =>
            new Currency {Code = model.Code, Name = model.Name, Rating = model.Rating};

        /// <summary>
        /// Converts App Model into DB Entity 
        /// </summary>
        /// <param name="model">App model</param>
        /// <returns>DB Entity</returns>
        public static CurrencyModel ToCurrencyModel(this Currency model) =>
            new CurrencyModel {Code = model.Code, Name = model.Name, Rating = model.Rating};

        /// <summary>
        /// Converts App model into Exchange model (Converter of currencies)
        /// Example:
        /// 1 EUR = 1.2 USD
        /// 10 EUR = 1.2 USD * 10 = 12 USD
        /// ...
        /// </summary>
        /// <param name="model">App Model</param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static ExchangeModel ToExchangeModel(this CurrencyModel model, double amount) =>
            new ExchangeModel()
            {
                Code = model.Code,
                Rating = model.Rating * amount
            };

        #region Searching flags for some codes

        internal static ImageSource GetImage(string str, string format)
        {
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource =
                new Uri($"{CommonData.IconFolder}{str}.{format}",
                    UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            return bimage;
        }
        /// <summary>
        /// Fing flag icon by code
        /// Example:
        /// Code 'USD' -> 'US' -> 'us' -> 'us'.png: 
        /// take first 2 letters and convert to the lower case
        /// later concat '.png' to end of taken letters
        /// </summary>
        /// <param name="currency">Currency</param>
        /// <returns></returns>
        public static ImageSource GetImage(ICurrency currency)
        {
            return GetImage(new string(currency.Code.ToLower().Take(2).ToArray()), "png");
        }

        #endregion
    }
}
