using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.DataManagers
{
    class ApiDataManagerForCurrencies:IDataManager<ApiCurrencyModel>
    {
        private readonly CurrencyModel[] _currencyModels;

        public ApiDataManagerForCurrencies(CurrencyModel[] currencyModels)
        {
            _currencyModels = currencyModels;
        }

        public void Save(ApiCurrencyModel data)
        {
            if (data == null) return;
            var uow = UnitOfWork.Instance;
            uow.DeleteCurrencies();
            foreach (var model in data.Quotes)
            {
                uow.Add(new Currency() {Code = model.Key, Rating = model.Value});
            }
            uow.Save();
        }

        public ApiCurrencyModel Upload()
        {
            CurrencyLayerProvider provider =
                new CurrencyLayerProvider(new HttpClient() { Timeout = TimeSpan.FromSeconds(10) });
            return  provider.GetLiveCurrencyModel(_currencyModels);
        }
    }
}