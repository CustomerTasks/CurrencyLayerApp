using System;
using System.Collections.Generic;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.DataManagers
{
    class LocalDataManagerForHistoricalData : IDataManager<Dictionary<DateTime, ApiCurrencyModel>>
    {
        public void Save(Dictionary<DateTime, ApiCurrencyModel> data)
        {
           
        }

        public Dictionary<DateTime, ApiCurrencyModel> Upload()
        {
            return Parsers.GetStoredHistoryData();
        }
    }
}