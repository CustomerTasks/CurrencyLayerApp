using System;
using System.Collections.Generic;
using System.Linq;
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
    class ApiDataManagerForHistoricalData :IDataManager<Dictionary<DateTime, ApiCurrencyModel>>
    {
        private CurrencyModel[] _currencyModels;

        /// <summary>
        /// Uploads data from API requests
        /// </summary>
        /// <param name="models">params for API</param>
        public ApiDataManagerForHistoricalData(params CurrencyModel[] models)
        {
            _currencyModels = models;
        }

        public void Save(Dictionary<DateTime, ApiCurrencyModel> data)
        {
            if (data == null || !data.Any()) return;
            var uow = UnitOfWork.Instance;
            //Rewrite historical data
            lock (uow)
            {
                uow.DeleteHistoricalData();
                var currencies = uow.GetCurrencies();
                if (currencies.Any() && data.Any())
                {
                    foreach (var historicalData in data)
                    {
                        foreach (var quote in historicalData.Value.Currencies)
                        {
                            if (currencies.Any(x => x.Code == quote.Key))
                            {
                                var cur = currencies.First(x => x.Code == quote.Key);
                                cur.HistoricalDatas
                                    .Add(new HistoricalData()
                                    {
                                        Date = historicalData.Key,
                                        Currency = cur,
                                        Rating = quote.Value
                                    });
                            }
                        }
                    }
                }
                uow.Save();
            }
        }

        public Dictionary<DateTime, ApiCurrencyModel> Upload()
        {
            CurrencyLayerProvider provider =
                new CurrencyLayerProvider(new HttpClient() { Timeout = TimeSpan.FromSeconds(10) });
            return 
                provider.GetHistoricalCurrencyModel(_currencyModels, DateTime.Now,
                    7);
        }
    }
}