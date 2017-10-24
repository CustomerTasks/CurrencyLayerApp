using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;
using CurrencyLayerApp.Resources.Strings;

namespace CurrencyLayerApp.ViewModels
{
    /// <summary>
    /// ViewModel for HistoricalDataPage.xaml
    /// </summary>
    class HistoricalDataViewModel : ViewModelBase, IInitializationManager
    {
        public HistoricalDataViewModel()
        {
            IsEnabled = false;
            Message = "Loading...";
        }
        #region <Fields>

        /// <summary>
        /// Selected currencies
        /// </summary>
        private ObservableCollection<CurrencyModel> _currencyModels;

        /// <summary>
        /// Header for Y-axis
        /// </summary>
        private string _description;

        private string _message;

        /// <summary>
        /// Currency Points (datetime, rating)
        /// </summary>
        private KeyValuePair<string, double>[] _chart;

        private CurrencyModel _currencyModelFrom;
        private CurrencyModel _currencyModelTo;

        /// <summary>
        /// Colection of historical data (datetime, currencies[])
        /// </summary>
        private Dictionary<DateTime, ApiCurrencyModel> _historicalData;

        /// <summary>
        /// Manager which saves/uploads data.
        /// </summary>
        private IDataManager<Dictionary<DateTime, ApiCurrencyModel>> _dataManager;

        /// <summary>
        /// Property for blocking UI or etc
        /// </summary>
        private bool _isEnabled;

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

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
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
            //For avoiding bugs & null collections/values
            if (_currencyModels == null || !_currencyModels.Any())
            {
                if (_currencyModels != null && _currencyModels.Any())
                {
                    var checkingModels =
                        new ObservableCollection<CurrencyModel>(CurrencyLayerApplication.CurrencyModels);
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

        /// <summary>
        /// Initializes and draws chart
        /// </summary>
        private void InitializeChart()
        {
            //For avoiding bugs & null collections/values
            if (!CurrencyModels.Any() || CurrencyModelFrom == null || CurrencyModelTo == null ||
                _historicalData == null)
                return;
            Description = $"{_currencyModelFrom.Code}/{_currencyModelTo.Code}";
            _chart = new KeyValuePair<string, double>[_historicalData.Count];
            int i = _chart.Length - 1;
            foreach (var model in _historicalData)
            {
                _chart[i--] = new KeyValuePair<string, double>(model.Key.ToString("dd/MM/yyyy"),
                    model.Value.Currencies[_currencyModelFrom.Code] / model.Value.Currencies[_currencyModelTo.Code]);
            }
            Chart = _chart;
        }

        /// <summary>
        /// Initializes models after applyying changes in Setting tab.
        /// </summary>
        private void InitializeModels()
        {
            CurrencyModels =
                new ObservableCollection<CurrencyModel>(CurrencyLayerApplication.CurrencyModels);
            IsEnabled = true;
        }

        /// <summary>
        /// Executes main task.
        /// </summary>
        protected override void Execute()
        {
            while (true)
            {
                if (Settings.Instance.IsFihished)
                {
                    DownloaderThread.Abort();
                    break;
                }
                Initialize();
                if (!Settings.Instance.IsConfigured ||  (_currencyModels == null || !_currencyModels.Any())) continue;
                
                UploadByManagers();                
                if (!IsCreated)
                {
                    CheckSelectedModels();
                    InitializeChart();
                    Task.Run(() => _dataManager.Save(_historicalData));
                    IsCreated = true;
                    Message = "Done";
                }
                
                CurrencyLayerApplication.ThreadSleep();
            }
        }

        /// <summary>
        /// Checks models are null after applying changes in Setting tab. 
        /// </summary>
        private void CheckSelectedModels()
        {
            var currencyModels = CurrencyLayerApplication.CurrencyModels;
            if (_historicalData != null && _historicalData.Any() && currencyModels.Any())
            {
                var currencies= _historicalData.First().Value.Currencies;
                //For avoiding bugs & null collections/values. 
                //Also checking containing some curency in last updated historical data.
                if ((_currencyModelFrom == null || _currencyModelTo == null) 
                    && currencyModels.Length== currencies.Values.Count
                    && currencyModels.Any(x => currencies.First(t=>t.Key == x.Code).Key == x.Code )
                    && currencyModels.Any(x => currencies.Last(t => t.Key == x.Code).Key == x.Code))
                {
                    CurrencyModelFrom =
                        currencyModels.First(x => currencies.First(t => t.Key == x.Code).Key == x.Code);
                    CurrencyModelTo =
                        currencyModels.First(x => currencies.First(t => t.Key == x.Code).Key == x.Code);
                    Message = "Done.";
                    IsEnabled = true;
                }
            }
        }

        /// <summary>
        /// Upload data from local DB or API.
        /// </summary>
        private void UploadByManagers()
        {
            //For avoiding bugs & null collections.  If this values aren't  uploded from db early.
            
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
                if(_historicalData==null)
                    Logger.SetLogMessage(MainLogMessages.EmptyHistory,Logger.Color.Red);
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
