using System;

namespace CurrencyLayerApp.DAL.Repositories
{
    /// <summary>
    /// Pattern 'Repository'
    /// See in README.md 
    /// </summary>
    /// <typeparam name="T">Entity</typeparam>
    internal interface IRepository<T> where T : class
    {
        /// <summary>
        /// INSERT Command
        /// </summary>
        /// <param name="item">entity</param>
        void Add(T item);

        /// <summary>
        /// DELETE Command
        /// </summary>
        /// <param name="func">Condition WHERE</param>
        void Delete(Func<T, bool> func);

        /// <summary>
        /// SELECT one entity by condition
        /// </summary>
        /// <param name="func">Condition WHERE</param>
        /// <returns>data</returns>
        T Get(Func<T, bool> func);

        /// <summary>
        /// SELECT * from entity
        /// </summary>
        /// <returns>All data</returns>
        T[] GetAll();

        /// <summary>
        /// TRUNCATE Table/ DELETE from Table
        /// </summary>
        void Truncate();

        /// <summary>
        /// Checks is table empty.
        /// </summary>
        /// <returns>result</returns>
        bool IsNotEmpty();
    }
}
