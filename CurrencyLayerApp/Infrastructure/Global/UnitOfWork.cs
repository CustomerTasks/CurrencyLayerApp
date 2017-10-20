using System;
using CurrencyLayerApp.DAL.Contexts;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.DAL.Repositories;

namespace CurrencyLayerApp.Infrastructure.Global
{
    sealed class UnitOfWork
    {
        private readonly CurrencyLayerContext _context;
        private static readonly Lazy<UnitOfWork> Lazy= new Lazy<UnitOfWork>(()=> new UnitOfWork());
        public static UnitOfWork Instance { get; } = Lazy.Value;

        private UnitOfWork()
        {
            _context = new CurrencyLayerContext();
            _repository = new CurrencyRepository(_context);
        }

        private readonly IRepository<Currency> _repository;

        public void Add(Currency currency)
        {
            _repository.Add(currency);
        }

        public Currency Get(Func<Currency, bool> func)
        {
            return _repository.Get(func);
        }
        public bool Any(Type type)
        {
            if (type == typeof(Currency))
                return _repository.NotEmpty();
            return false;
        }
        public void Delete(Func<Currency,bool> func)
        {
            _repository.Delete(func);
        }

        public void DeleteCurrencies()
        {
           _repository.Truncate();
        }

        public void Save()
        {
            _repository.Save();
        }

        public Currency[] GetCurrencies()
        {
            return _repository.GetAll();
        }
    }
}
