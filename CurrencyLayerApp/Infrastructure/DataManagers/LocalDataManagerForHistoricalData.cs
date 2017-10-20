using System;
using System.Collections.Generic;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure
{
    class LocalDataManagerForHistoricalData : IDataManager<Dictionary<DateTime, ApiCurrencyModel>>
    {
        private CurrencyModel _currencyModelFrom;
        private CurrencyModel _currencyModelTo;

        public LocalDataManagerForHistoricalData(CurrencyModel currencyModelFrom, CurrencyModel currencyModelTo)
        {
            _currencyModelFrom = currencyModelFrom;
            _currencyModelTo = currencyModelTo;
        }

        public void Save(Dictionary<DateTime, ApiCurrencyModel> data)
        {
           
        }

        public Dictionary<DateTime, ApiCurrencyModel> Upload()
        {
            return Parsers.GetStoredHistoryData();
        }
    }
}