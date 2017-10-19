using System.IO;
using System.Linq;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Helpers
{
    public static class Parsers
    {
        public static CurrencyModel[] ParseCurrencyModels(string path = null)
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
    }
}
