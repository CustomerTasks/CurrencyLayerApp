using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.DataManagers
{
    internal class LocalDataManagerForCurrencies : IDataManager<ApiCurrencyModel>
    {
        private readonly CurrencyModel[] _currencyModels;

        public LocalDataManagerForCurrencies(CurrencyModel[] currencyModels)
        {
            _currencyModels = currencyModels;
        }

        public void Save(ApiCurrencyModel data)
        {
            
        }

        public ApiCurrencyModel Upload()
        {
            var result = new ApiCurrencyModel();
            foreach (var currencyModel in _currencyModels)
            {
                result.Quotes.Add(currencyModel.Code, currencyModel.Rating);
            }
            return result;
        }
    }
}