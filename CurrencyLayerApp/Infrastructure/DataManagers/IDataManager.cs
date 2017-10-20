namespace CurrencyLayerApp.Infrastructure.DataManagers
{
    interface IDataManager<T> where T:class 
    {
        void Save(T data);
        T Upload();
    }
}
