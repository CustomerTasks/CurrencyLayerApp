using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class HistoricalDataViewModel : ViewModelBase, IDownloader
    {
        public HistoricalDataViewModel()
        {
            Thread = new Thread(DownloadData);
            Thread.Start();
            _currencyModels = new ObservableCollection<CurrencyModel>(Parsers.GetStoredModels(true));
            if (_currencyModels.Any())
            {
                CurrencyModelFrom = _currencyModels.First();
                CurrencyModelTo = _currencyModels.Count > 1 ? _currencyModels.Last() : _currencyModels.First();
            }

        }

        #region <Fields>

        private ObservableCollection<CurrencyModel> _currencyModels;
        private string _description;
        private KeyValuePair<string, double>[] _chart;
        private CurrencyModel _currencyModelFrom;
        private CurrencyModel _currencyModelTo;
        private Dictionary<DateTime, ApiCurrencyModel> _historicalData;
        private IDataManager<Dictionary<DateTime, ApiCurrencyModel>> _dataManager;

        #endregion

        #region <Properties>

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

        public Thread Thread { get; set; }


        #endregion

        #region <Methods> 

        private void InitializeChart()
        {
            if (!_currencyModels.Any())
                return;
            Description = $"{_currencyModelFrom.Code}/{_currencyModelTo.Code}";
            _chart = new KeyValuePair<string, double>[_historicalData.Count];
            int i = _chart.Length - 1;
            foreach (var model in _historicalData)
            {
                _chart[i--] = new KeyValuePair<string, double>(model.Key.ToString("yyyy/MM/dd"),
                    model.Value.Quotes[_currencyModelFrom.Code] / model.Value.Quotes[_currencyModelTo.Code]);
            }
            Chart = _chart;
        }

        public void DownloadData()
        {
            while (true)
            {
                try
                {
                    if (!Settings.Instance.IsPrepared || !_currencyModels.Any())
                    {
                        //_dataManager = new ApiDataManagerForHistoricalData();
                        return;
                    }
                    _dataManager = new ApiDataManagerForHistoricalData(CurrencyModelFrom, CurrencyModelTo);
                    _historicalData = _dataManager.Upload();
                    if (_historicalData == null || !_historicalData.Any())
                    {
                        _dataManager = new LocalDataManagerForHistoricalData(CurrencyModelFrom, CurrencyModelTo);
                        _historicalData = _dataManager.Upload();
                    }
                    {
                        var currencyModels = Parsers.GetStoredModels(true);
                        _historicalData = _dataManager.Upload();
                        if (_currencyModelFrom == null || _currencyModelTo == null)
                        {
                            _currencyModelFrom =
                                currencyModels.First(x => _historicalData.First().Value.Code == x.Code);
                            _currencyModelTo =
                                currencyModels.First(x => _historicalData.Last().Value.Code == x.Code);
                        }
                        _historicalData = Parsers.GetStoredHistoryData();
                    }
                    Task.Run(() => _dataManager.Save(_historicalData));
                    InitializeChart();
                    System.Threading.Thread.Sleep(Settings.Instance.TimeBetweenCalls * 1000);
                }
                catch (Exception e)
                {
                    //ignored
                }
            }
        }

        #endregion

        #region <Additional>



        #endregion
        
        /*private void Save()
        {
            Task.Run(() =>
            {
                var uow = UnitOfWork.Instance;
                uow.DeleteHistoricalData();
                var currencies = uow.GetCurrencies();
                if (currencies.Any() && _historicalData.Any())
                {
                    foreach (var historicalData in _historicalData)
                    {
                        foreach (var quote in historicalData.Value.Quotes)
                        {
                            if (currencies.Any(x => x.Code == quote.Key))
                            {
                                var cur = currencies.First(x => x.Code == quote.Key);
                                cur.Rating = quote.Value;
                                cur.HistoricalDatas
                                    .Add(new HistoricalData() {Date = historicalData.Key, Currency = cur });
                            }
                        }
                    }
                }
                uow.Save();
            });
        }*/
    }
}
