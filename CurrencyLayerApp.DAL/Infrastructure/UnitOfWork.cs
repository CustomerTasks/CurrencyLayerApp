using System;
using CurrencyLayerApp.DAL.Contexts;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.DAL.Repositories;

namespace CurrencyLayerApp.DAL.Infrastructure
{
    public sealed class UnitOfWork:IDisposable
    {
        private UnitOfWork()
        {
            var context = new CurrencyLayerContext();
            _currencyRepository = new CurrencyRepository(context);
            _historicalRepository = new HistoricalDataRepository(context);
        }

        #region <Fields>

        private static readonly Lazy<UnitOfWork> Lazy = new Lazy<UnitOfWork>(() => new UnitOfWork());
        private readonly IRepository<Currency> _currencyRepository;
        private readonly IRepository<HistoricalData> _historicalRepository;

        #endregion

        #region <Properties>

        public static UnitOfWork Instance { get; } = Lazy.Value;
        public bool IsDisposed { get; private set; } = false;

        #endregion

        #region <Methods>

        #region Adding

        public void Add(Currency currency)
        {
            _currencyRepository.Add(currency);
        }

        public void Add(HistoricalData currency)
        {
            _historicalRepository.Add(currency);
        }

        #endregion

        #region Getting

        public Currency Get(Func<Currency, bool> func)
        {
            return _currencyRepository.Get(func);
        }

        public HistoricalData Get(Func<HistoricalData, bool> func)
        {
            return _historicalRepository.Get(func);
        }

        public Currency[] GetCurrencies()
        {
            return _currencyRepository.GetAll();
        }

        public HistoricalData[] GetHistoricalData()
        {
            return _historicalRepository.GetAll();
        }

        #endregion

        #region Deleting

        public void Delete(Func<Currency, bool> func)
        {
            _currencyRepository.Delete(func);
        }

        public void Delete(Func<HistoricalData, bool> func)
        {
            _historicalRepository.Delete(func);
        }

        #endregion

        #region Truncating

        public void DeleteCurrencies()
        {
            _currencyRepository.Truncate();
        }

        public void DeleteHistoricalData()
        {
            _historicalRepository.Truncate();
        }

        #endregion

        public bool Any(Type type)
        {
            if (type == typeof(Currency))
                return _currencyRepository.NotEmpty();
            if (type == typeof(HistoricalData))
                return _historicalRepository.NotEmpty();
            return false;
        }

        public void Save()
        {
            _currencyRepository.Save();
            _historicalRepository.Save();
        }

        #endregion

        private void ForceDispose()
        {
            if (IsDisposed)
            {
                IsDisposed = true;
                _currencyRepository?.Dispose();
                _historicalRepository?.Dispose();
                GC.Collect();
            }
        }
        public void Dispose()
        {
            if (IsDisposed)
                return;
            ForceDispose();
        }
    }
}
