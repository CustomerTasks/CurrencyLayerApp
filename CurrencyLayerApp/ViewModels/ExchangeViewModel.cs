using System;
using System.Collections.ObjectModel;
using System.Linq;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;
using static CurrencyLayerApp.Infrastructure.CurrencyLayerApplication;

namespace CurrencyLayerApp.ViewModels
{
    /// <summary>
    /// ViewModel for CurrentDataPage.xaml
    /// </summary>
    internal class ExchangeViewModel : ViewModelBase, IInitializationManager
    {
        #region <Fields>

        /// <summary>
        /// Value which we want to convert to other currencies.
        /// </summary>
        private double _currencyValue;

        /// <summary>
        /// General selected currencies.
        /// </summary>
        private ObservableCollection<CurrencyModel> _currencyModels;

        /// <summary>
        /// Selected curency which we want to get converted values in other values.
        /// </summary>
        private CurrencyModel _selectedCurrencyModel;

        /// <summary>
        /// Converted currencies without _selectedCurrencyModel.
        /// </summary>
        private ObservableCollection<ExchangeModel> _exchangeModels;

        /// <summary>
        /// Property for blocking UI or etc
        /// </summary>
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

        public CurrencyModel SelectedCurrencyModel
        {
            get { return _selectedCurrencyModel; }
            set
            {
                _selectedCurrencyModel = value;
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

        /// <summary>
        /// Initializes data & contexts by selected currencies.
        /// It need after apllying changes in Settings (changing currency selections)
        /// </summary>
        public void Initialize()
        {
            if (Settings.Instance.IsConfigured)
            {
                CurrencyModels = new ObservableCollection<CurrencyModel>(CurrencyLayerApplication.CurrencyModels);
                if (CurrencyModels != null && CurrencyModels.Any())
                {
                    SelectedCurrencyModel = _currencyModels.First();
                    CurrencyValue = 1.0;
                }
                IsEnabled = true;
            }
            else
            {
                IsEnabled = false;
            }
        }

        /// <summary>
        /// Execures main task.
        /// </summary>
        protected override void Execute()
        {
            while (true)
            {
                if (Settings.Instance.IsFihished)
                {
                    Thread.Abort();
                    break;
                }
                if (!Settings.Instance.IsConfigured)
                    return;
                Initialize();
                Calculation();
                ThreadSleep();
            }
        }

        /// <summary>
        /// Converts value for other currencies.
        /// </summary>
        private void Calculation()
        {
            if (!CurrencyModels.Any()) return;
            if (SelectedCurrencyModel == null) SelectedCurrencyModel = CurrencyModels.First();

            //Takes last updated data for converting.
            var dataManager = new LocalDataManagerForCurrencies();
            var lastHistory = dataManager.Upload();
            foreach (var quote in lastHistory.Currencies)
            {
                if (CurrencyModels.Any(x => x.Code == quote.Key))
                {
                    CurrencyModels.First(x => x.Code == quote.Key).Rating = quote.Value;
                }
            }

            //Filter currencies without CurrencyModel
            var forCalculating = CurrencyModels.Where(x => x.Code != SelectedCurrencyModel.Code);
            SelectedCurrencyModel.Rating = CurrencyModels.First(x => x.Code == SelectedCurrencyModel.Code).Rating;

            //Converting. Formula: ConvertedCurrency[i] =  (CurrencyValue * CurrencyModels[i])/ SelectedCurrency.
            ExchangeModels = new ObservableCollection<ExchangeModel>(forCalculating.Select(x =>
                x.ToExchangeModel(Math.Round(CurrencyValue / SelectedCurrencyModel.Rating, 5))));
        }

        #endregion

    }
}
