using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Helpers
{
    public static class Parsers
    {
        private static CurrencyModel[] ParseCurrencyModels(string path = null)
        {
            if (string.IsNullOrEmpty(path))
            {
                path = CommonData.CurrenciesAssetFile;
            }
            var fileinfo = new FileInfo(path);
            if (!fileinfo.Exists)
            {
                return null;
            }
            var lines = File.ReadAllLines(path);
            CurrencyModel[] models = new CurrencyModel[lines.Length];
            for (var index = 0; index < lines.Length; index++)
            {
                var line = lines[index];
                var splited = line.Split('\t');
                models[index] = new CurrencyModel() {Code = splited.First(), Name = splited.Last()};
            }
            return models;
        }

        public static CurrencyModel[] GetStoredModels(bool selected=false)
        {
            var result = Parsers.ParseCurrencyModels();
            var uow = UnitOfWork.Instance;
            if (uow.Any(typeof(Currency)))
            {
                var models = uow.GetCurrencies();
                foreach (var model in models)
                {
                    if (result.Any(x => x.Code == model.Code))
                    {
                        var first = result.First(x => x.Code == model.Code);
                        var currencyModel = first;
                        currencyModel.IsSelected = true;
                        currencyModel.Rating = first.Rating;
                    }
                }
            }
            if (selected)
            {
                result = result.Where(x => x.IsSelected).ToArray();
            }
            return result;
        }
        public static Dictionary<DateTime, ApiCurrencyModel> GetStoredHistoryData()
        {
            var uow = UnitOfWork.Instance;
            var result = new Dictionary<DateTime, ApiCurrencyModel>();
            if (uow.Any(typeof(HistoricalData)))
            {
                var models = uow.GetHistoricalData();
                foreach (var model in models)
                {
                    var date = model.Date.Value;
                    var modelCurrency = model.Currency;
                    if (result.ContainsKey(date)){
                        result[date].Code = "USD";
                        result[date].Quotes.Add(modelCurrency.Code, modelCurrency.Rating);
                    }
                    else
                    {
                        result.Add(date,new ApiCurrencyModel(){Code = "USD",Quotes = {{ modelCurrency.Code, modelCurrency.Rating } }});
                    }
                }
                return result;
            }
            return null;
        }
    }
}
