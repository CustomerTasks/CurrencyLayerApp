using System;
using System.Data.Entity;
using System.Linq;
using CurrencyLayerApp.DAL.Contexts;
using CurrencyLayerApp.DAL.Entities;

namespace CurrencyLayerApp.DAL.Repositories
{
    internal class HistoricalDataRepository : IRepository<HistoricalData>
    {
        private readonly CurrencyLayerContext _context;

        public HistoricalDataRepository(CurrencyLayerContext context)
        {
            _context = context;
            Set = _context.HistoricalDatas;
        }

        public DbSet<HistoricalData> Set { get; set; }

        public void Add(HistoricalData item)
        {
            Set.Add(item);
        }

        public void Delete(Func<HistoricalData, bool> func)
        {
            var found = Get(func);
            if (found == null)
            {
                return;
            }
            Set.Remove(found);
        }

        public HistoricalData Get(Func<HistoricalData, bool> func)
        {
            return Set.FirstOrDefault(func);
        }
        public HistoricalData[] GetAll()
        {
            return Set.ToArray();
        }

        public void Truncate()
        {
            Set.RemoveRange(Set);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
        public bool NotEmpty()
        {
            return Set.Any();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}