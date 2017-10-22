namespace CurrencyLayerApp.Abstractions
{
    /// <summary>
    /// Interface which provides methods for uploading data from server/local memory and saving.
    /// (Strategy Pattern)
    /// </summary>
    /// <typeparam name="T">type of data</typeparam>
    interface IDataManager<T> where T:class 
    {
        /// <summary>
        /// Saves into local DB
        /// </summary>
        /// <param name="data"></param>
        void Save(T data);
        /// <summary>
        /// Uploads data from local DB/api
        /// </summary>
        /// <returns></returns>
        T Upload();
    }
}
