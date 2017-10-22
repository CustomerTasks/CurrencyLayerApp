using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;
using static CurrencyLayerApp.Infrastructure.CurrencyLayerApplication;

namespace CurrencyLayerApp.ViewModels
{
    internal class ExchangeViewModel : ViewModelBase, IInitializationManager
    {
        #region <Fields>

        private double _currencyValue;
        private ObservableCollection<CurrencyModel> _currencyModels;
        private CurrencyModel _currencyModel;
        private ObservableCollection<ExchangeModel> _exchangeModels;
        private bool _isEnabled;

        #endregion

        #region <Properties>

        public double CurrencyValue
        {
            get { return _currencyValue; }
            set
            {
                _currencyValue = value;
                Calculation();
                OnPropertyChanged();
            }
        }

        public ObservableCollection<CurrencyModel> CurrencyModels
        {
            get { return _currencyModels; }
            set
            {
                _currencyModels = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ExchangeModel> ExchangeModels
        {
            get { return _exchangeModels; }
            set
            {
                _exchangeModels = value;
                OnPropertyChanged();
            }
        }

        public CurrencyModel CurrencyModel
        {
            get { return _currencyModel; }
            set
            {
                _currencyModel = value;
                Calculation();
                OnPropertyChanged();
            }
        }

        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged();
            }
        }
         
        #endregion

        #region <Methods>

        public void Initialize()
        {
            if (Settings.Instance.IsConfigured)
            {
                CurrencyModels = new ObservableCollection<CurrencyModel>(CurrencyLayerApplication.CurrencyModels);
                if (CurrencyModels != null && CurrencyModels.Any())
                {
                    CurrencyModel = _currencyModels.First();
                    CurrencyValue = 1.0;
                }
                IsEnabled = true;
            }
            else
            {
                IsEnabled = false;
            }
        }

        protected override void ThreadMethod()
        {
            while (true)
            {
                if (Settings.Instance.IsFihished)
                {
                    Thread.Abort();
                    break;
                }
                if (!Settings.Instance.IsConfigured )
                    return;
                Initialize();
                Calculation();
                ThreadSleep();
            }
        }

        private void Calculation()
        {
            if (!CurrencyModels.Any()) return;
            if (CurrencyModel == null) CurrencyModel = CurrencyModels.First();
            var dataManager = new LocalDataManagerForCurrencies();
            var lastHistory = dataManager.Upload();
            foreach (var quote in lastHistory.Currencies)
            {
                if (CurrencyModels.Any(x => x.Code == quote.Key))
                {
                    CurrencyModels.First(x => x.Code == quote.Key).Rating = quote.Value;
                }
            }
            var forCalculating = CurrencyModels.Where(x => x.Code != CurrencyModel.Code);
            CurrencyModel.Rating = CurrencyModels.First(x => x.Code == CurrencyModel.Code).Rating;
            ExchangeModels = new ObservableCollection<ExchangeModel>(forCalculating.Select(x =>
                x.ToExchangeModel(Math.Round(CurrencyValue / CurrencyModel.Rating,5))));
        }

        #endregion
        
    }
}
