using System;
using System.Net.Http;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.DataManagers
{
    /// <inheritdoc />
    /// <summary>
    /// More see in IDataProvider
    /// </summary>
    class ApiDataManagerForCurrencies:IDataManager<ApiCurrencyModel>
    {
        private readonly CurrencyModel[] _currencyModels;
        /// <summary>
        /// Uploads live currencies ratings from API requests
        /// </summary>
        /// <param name="currencyModels">params for API</param>
        public ApiDataManagerForCurrencies(CurrencyModel[] currencyModels)
        {
            _currencyModels = currencyModels;
        }

        public void Save(ApiCurrencyModel data)
        {
            if (data == null) return;
            var uow = UnitOfWork.Instance;
            //Rewrite currency data
            lock (uow)
            {
                uow.DeleteCurrencies();
                foreach (var model in data.Currencies)
                {
                    uow.Add(new Currency() {Code = model.Key, Rating = model.Value});
                }
                uow.Save();
            }
        }

        public ApiCurrencyModel Upload()
        {
            CurrencyLayerProvider provider =
                new CurrencyLayerProvider(new HttpClient() { Timeout = TimeSpan.FromSeconds(10) });
            return  provider.GetLiveCurrencyModel(_currencyModels);
        }
    }
}