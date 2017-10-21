using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.DataManagers
{
    internal class LocalDataManagerForCurrencies : IDataManager<ApiCurrencyModel>
    {
        public void Save(ApiCurrencyModel data)
        {
            
        }

        public ApiCurrencyModel Upload()
        {
            var result = new ApiCurrencyModel();
            var uow = UnitOfWork.Instance;
            foreach (var currencyModel in uow.GetCurrencies())
            {
                result.Quotes.Add(currencyModel.Code, currencyModel.Rating);
            }
            return result;
        }
    }
}