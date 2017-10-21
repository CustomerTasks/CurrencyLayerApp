using System.Collections.ObjectModel;
using System.Linq;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    internal class ExchangeViewModel : ViewModelBase
    {
        public ExchangeViewModel()
        {
            CurrencyModels = new ObservableCollection<CurrencyModel>(Parsers.GetStoredModels(true));
            CurrencyModel = _currencyModels.First();
            CurrencyValue = 1.0;
        }

        #region <Fields>

        private double _currencyValue;
        private ObservableCollection<CurrencyModel> _currencyModels;
        private CurrencyModel _currencyModel;
        private ObservableCollection<ExchangeModel> _exchangeModels;

        #endregion

        #region <Properties>

        public double CurrencyValue
        {
            get { return _currencyValue; }
            set
            {
                _currencyValue = value;
                Calculate();
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
                Calculate();
                OnPropertyChanged();
            }
        }



        #endregion

        #region <Methods>

        private void Calculate()
        {
            var dataManager = new LocalDataManagerForCurrencies();
            var lastHistory = dataManager.Upload();
            foreach (var quote in lastHistory.Quotes)
            {
                CurrencyModels.First(x => x.Code == quote.Key).Rating = quote.Value;
            }
            var forCalculating = CurrencyModels.Where(x => x.Code != CurrencyModel.Code);
            CurrencyModel.Rating = CurrencyModels.First(x => x.Code == CurrencyModel.Code).Rating;
            ExchangeModels = new ObservableCollection<ExchangeModel>(forCalculating.Select(x =>
                x.ToExchangeModel(CurrencyValue/CurrencyModel.Rating)));
        }

        #endregion


    }
}
