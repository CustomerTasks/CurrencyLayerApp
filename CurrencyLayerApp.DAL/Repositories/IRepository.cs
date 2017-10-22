using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLayerApp.DAL.Repositories
{
    internal interface IRepository<T> where T:class
    {
        void Add(T item);
        void Delete(Func<T, bool> func);
        T Get(Func<T, bool> func);
        T[] GetAll();
        void Truncate();
        bool IsNotEmpty();
    }
}
