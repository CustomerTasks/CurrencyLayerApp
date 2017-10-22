using System;
using System.Data.Entity;
using System.Linq;
using CurrencyLayerApp.DAL.Contexts;
using CurrencyLayerApp.DAL.Entities;

namespace CurrencyLayerApp.DAL.Repositories
{
    internal class CurrencyRepository : IRepository<Currency>
    {

        public CurrencyRepository(CurrencyLayerContext context)
        {
            Set = context.Currencies;
        }

        public DbSet<Currency> Set { get; set; }

        public void Add(Currency item)
        {
            Set.Add(item);
        }

        public void Delete(Func<Currency, bool> func)
        {
            var found = Get(func);
            if (found == null)
            {
                return;
            }
            Set.Remove(found);
        }

        public Currency Get(Func<Currency, bool> func)
        {
            return Set.FirstOrDefault(func);
        }
        public Currency[] GetAll()
        {
            return Set.ToArray();
        }

        public void Truncate()
        {
            Set.RemoveRange(Set);
        }
        public bool IsNotEmpty()
        {
            return Set.Any();
        }

    }
}