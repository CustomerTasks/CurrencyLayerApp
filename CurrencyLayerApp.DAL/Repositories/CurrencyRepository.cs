using System;
using System.Data.Entity;
using System.Linq;
using CurrencyLayerApp.DAL.Contexts;
using CurrencyLayerApp.DAL.Entities;

namespace CurrencyLayerApp.DAL.Repositories
{
    public class CurrencyRepository : IRepository<Currency>
    {
        private readonly CurrencyLayerContext _context;

        public CurrencyRepository(CurrencyLayerContext context)
        {
            _context = context;
            Set = _context.Currencies;
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

        public void Truncate()
        {
            Set.RemoveRange(Set);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}