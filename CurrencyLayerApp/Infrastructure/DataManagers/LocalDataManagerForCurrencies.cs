using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.DataManagers
{
    internal class LocalDataManagerForCurrencies : IDataManager<ApiCurrencyModel>
    {
        public void Save(ApiCurrencyModel data)
        {
            //ignored
        }
        public ApiCurrencyModel Upload()
        {
            var result = new ApiCurrencyModel();
            var uow = UnitOfWork.Instance;
            foreach (var currencyModel in uow.GetCurrencies())
            {
                result.Currencies.Add(currencyModel.Code, currencyModel.Rating);
            }
            return result;
        }
    }
}