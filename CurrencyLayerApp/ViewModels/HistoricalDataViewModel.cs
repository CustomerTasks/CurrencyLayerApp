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
            CurrencyModels = new ObservableCollection<CurrencyModel>(Parsers.GetStoredModels(true));
            if (CurrencyModels.Any())
            {
                CurrencyModelFrom = CurrencyModels.First();
                CurrencyModelTo = CurrencyModels.Count > 1 ? CurrencyModels.Last() : CurrencyModels.First();
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
                InitializeChart();
                OnPropertyChanged();
            }
        }

        public CurrencyModel CurrencyModelTo
        {
            get { return _currencyModelTo; }
            set
            {
                _currencyModelTo = value;
                InitializeChart();
                OnPropertyChanged();
            }
        }

        public Thread Thread { get; set; }

        public ObservableCollection<CurrencyModel> CurrencyModels
        {
            get { return _currencyModels; }
            set
            {
                _currencyModels = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region <Methods> 

        private void InitializeChart()
        {
            if (!CurrencyModels.Any() || CurrencyModelFrom==null || CurrencyModelTo==null || _historicalData==null)
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
                if (Settings.Instance.IsFihished)
                {
                    Thread.Abort();
                    break;
                }
                try
                {
                    if (!Settings.Instance.IsPrepared || !CurrencyModels.Any())
                    {
                        return;
                    }
                    _dataManager = new ApiDataManagerForHistoricalData(_currencyModels.ToArray());
                    _historicalData = _dataManager.Upload();
                    if (_historicalData == null || !_historicalData.Any())
                    {
                        _dataManager = new LocalDataManagerForHistoricalData(CurrencyModelFrom, CurrencyModelTo);
                        _historicalData = _dataManager.Upload();
                    }
                    else
                    {
                        Task.Run(() => _dataManager.Save(_historicalData));
                    }
                        var currencyModels = Parsers.GetStoredModels(true);
                        if (_currencyModelFrom == null || _currencyModelTo == null)
                        {
                            CurrencyModelFrom =
                                currencyModels.First(x => _historicalData.First().Value.Code == x.Code);
                            CurrencyModelTo =
                                currencyModels.First(x => _historicalData.Last().Value.Code == x.Code);
                        }
                    
                    
                    InitializeChart();
                    Thread.Sleep(Settings.Instance.TimeBetweenCalls * 1000);
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
