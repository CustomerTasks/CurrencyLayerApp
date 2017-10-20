using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CurrencyLayerApp.DAL.Entities;
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
            if (selected && uow.Any(typeof(Currency)))
            {
                var models = uow.GetCurrencies();
                foreach (var model in models)
                {
                    if (result.Any(x => x.Code == model.Code))
                    {
                        result.First(x => x.Code == model.Code).IsSelected = true;
                    }
                }
                result = result.Where(x => x.IsSelected).ToArray();
            }
            return result;
        }
        
    }
}
