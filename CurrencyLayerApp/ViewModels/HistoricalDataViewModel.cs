using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows.Input;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class HistoricalDataViewModel : ViewModelBase
    {
        private ObservableCollection<CurrencyModel> _currencyModels;
        private string _description;
        private KeyValuePair<string, double>[] _chart;
        private CurrencyModel _currencyModelFrom;
        private CurrencyModel _currencyModelTo;
        private ICommand _refresh;

        public HistoricalDataViewModel()
        {
            CurrencyModels = new ObservableCollection<CurrencyModel>(Parsers.GetStoredModels(true));
            CurrencyModelFrom = CurrencyModels.First();
            CurrencyModelTo = CurrencyModels.Count > 1 ? CurrencyModels.Last() : CurrencyModels.First();
            _refresh=new Command(InitializeChart);
            InitializeChart();
        }

        private void InitializeChart()
        {
            Description = $"{_currencyModelFrom.Code}/{_currencyModelTo.Code}";
            if(Settings.Instance.ApiKey==null) return;
            CurrencyLayerProvider provider = new CurrencyLayerProvider(new HttpClient());
            var res = provider.GetHistoricalCurrencyModel(new[] {_currencyModelFrom, _currencyModelTo}, DateTime.Now, 14);
            _chart=new KeyValuePair<string, double>[res.Count];
            int i = _chart.Length-1;
            foreach (var model in res)
            {
                _chart[i--] = new KeyValuePair<string, double>(model.Key.Replace('-','/'), model.Value.Quotes[_currencyModelFrom.Code]/ model.Value.Quotes[_currencyModelTo.Code]);
            }
            Chart = _chart;
        }


        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged();
            }
        }

        public KeyValuePair<string, double>[] Chart
        {
            get { return _chart; }
            set
            {
                _chart = value;
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

        public CurrencyModel CurrencyModelFrom
        {
            get { return _currencyModelFrom; }
            set
            {
                _currencyModelFrom = value;
                OnPropertyChanged();
            }
        }

        public CurrencyModel CurrencyModelTo
        {
            get { return _currencyModelTo; }
            set
            {
                _currencyModelTo = value;
                OnPropertyChanged();
            }
        }

        public ICommand Refresh
        {
            get { return _refresh; }
            set
            {
                _refresh = value;
                OnPropertyChanged();
            }
        }
    }
}
