using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.DataVisualization.Charting;
using System.Windows.Media;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class HistoricalDataViewModel : ViewModelBase, IDownloader,IInitializationManager
    {
        private readonly AreaSeries _areaSerie;

        public HistoricalDataViewModel()
        {
            Thread = new Thread(DownloadData);
            Thread.Start();
            
        }

        #region <Fields>

        private ObservableCollection<CurrencyModel> _currencyModels;
        private string _description;
        private KeyValuePair<string, double>[] _chart;
        private CurrencyModel _currencyModelFrom;
        private CurrencyModel _currencyModelTo;
        private Dictionary<DateTime, ApiCurrencyModel> _historicalData;
        private IDataManager<Dictionary<DateTime, ApiCurrencyModel>> _dataManager;
        private bool _isEnabled;
        private double _max;
        private double min;

        public HistoricalDataViewModel(AreaSeries areaSerie):this()
        {
            _areaSerie = areaSerie;
        }

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
            if (_currencyModels == null || !_currencyModels.Any())
            {
                if (_currencyModels != null && _currencyModels.Any())
                {
                    var checkingModels = new ObservableCollection<CurrencyModel>(CurrencyLayerApplication.CurrencyModels);
                    if (_currencyModels.Count == checkingModels.Count)
                    {
                        int count = 0;
                        for (var i = 0; i < checkingModels.Count; i++)
                        {
                            if (_currencyModels[i].Code == checkingModels[i].Code)
                                count++;
                        }
                        if (count != checkingModels.Count)
                        {
                            InitializeModels();
                        }
                        return;
                    }
                }
                InitializeModels();
            }
            InitializeModels();
        }
        private void InitializeChart()
        {
            if (!CurrencyModels.Any() || CurrencyModelFrom==null || CurrencyModelTo==null || _historicalData==null)
                return;
            Description = $"{_currencyModelFrom.Code}/{_currencyModelTo.Code}";
            _chart = new KeyValuePair<string, double>[_historicalData.Count];
            int i = _chart.Length - 1;
            foreach (var model in _historicalData)
            {
                _chart[i--] = new KeyValuePair<string, double>(model.Key.ToString("dd/MM/yyyy"),
                    model.Value.Quotes[_currencyModelFrom.Code] / model.Value.Quotes[_currencyModelTo.Code]);
            }
            /*_areaSerie.Background = new SolidColorBrush(Color.FromArgb(172, 32, 178, 170));
            var linearAxis = ((LinearAxis) _areaSerie.DependentRangeAxis);
            linearAxis.Minimum = _chart.Min(x => x.Value);
            linearAxis.Maximum = _chart.Max(x => x.Value);*/
            Chart = _chart;
        }
        private void InitializeModels()
        {
            CurrencyModels =
                new ObservableCollection<CurrencyModel>(CurrencyLayerApplication.CurrencyModels);
            
            IsEnabled = true;
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
                    if (!Settings.Instance.IsConfigured) return;
                    Initialize();
                    DownloadByManagers();
                    if (!IsCreated)
                    {
                        Calculation();
                        InitializeChart();
                        Task.Run(() => _dataManager.Save(_historicalData));
                        IsCreated = true;
                    }
                    CurrencyLayerApplication.ThreadSleep();
                }
                catch (Exception e)
                {
                    //ignored
                }
            }
        }

        private void Calculation()
        {
            var currencyModels = CurrencyLayerApplication.CurrencyModels;
            if (_currencyModelFrom == null || _currencyModelTo == null)
            {
                CurrencyModelFrom =
                    currencyModels.First(x => _historicalData.First().Value.Code == x.Code);
                CurrencyModelTo =
                    currencyModels.First(x => _historicalData.Last().Value.Code == x.Code);
            }
        }

        private void DownloadByManagers()
        {
            _dataManager = new ApiDataManagerForHistoricalData(_currencyModels.ToArray());
            var downloaded = _dataManager.Upload();
            if (downloaded != null)
            {
                IsCreated = false;
                _historicalData = downloaded;
            }

            if (_historicalData == null || !_historicalData.Any())
            {
                _dataManager = new LocalDataManagerForHistoricalData();
                _historicalData = _dataManager.Upload();
            }
            else
            {
                IsCreated = false;
            }
        }

        #endregion

        #region <Additional>



        #endregion

    }
}
