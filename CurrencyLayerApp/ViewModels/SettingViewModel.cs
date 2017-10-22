using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class SettingViewModel : ViewModelBase
    {
        public SettingViewModel()
        {
            _currencyModels = new ObservableCollection<CurrencyModel>(Parsers.GetStoredModels());
            SearchField = string.Empty;
            InitCheckBoxs();
            SaveChanges = new CommandBase(() => Task.Run(() => Save()));
            SetDefaultValues = new CommandBase(SetDefault);
            ApiKey = Settings.Instance.ApiKey;
            Time = Settings.Instance.TimeBetweenCalls;
        }

        #region <Fields>
        /// <summary>
        /// All currencies from file Currencies.txt
        /// </summary>
        private ObservableCollection<CurrencyModel> _currencyModels;
        /// <summary>
        /// Filtered data (search, first selected, later non-selected).
        /// </summary>
        private ObservableCollection<CurrencyModel> _filteredModels;
        /// <summary>
        /// Button event "Save".
        /// </summary>
        private ICommand _savechanges;
        /// <summary>
        /// API key.
        /// </summary>
        private string _apiKey;
        /// <summary>
        /// Time between API calls.
        /// </summary>
        private int _time = 10;
        /// <summary>
        /// Button event "Default".
        /// </summary>
        private ICommand _setDefaultValues;
        /// <summary>
        /// SubString for cearch currencies.
        /// </summary>
        private string _searchField;
        /// <summary>
        /// Maximum count of selected currencies
        /// </summary>
        private const int MaxCurrencies = 7;

        #endregion

        #region <Properties>

        public ObservableCollection<CurrencyModel> CurrencyModels
        {
            get { return _currencyModels; }
            set
            {
                _currencyModels = value;
                OnPropertyChanged();
                FilterSearchedResult();
            }
        }

        public ICommand SaveChanges
        {
            get { return _savechanges; }
            set
            {
                _savechanges = value;
                OnPropertyChanged();
            }
        }

        public string ApiKey
        {
            get { return _apiKey; }
            set
            {
                _apiKey = value;
                Settings.Instance.ApiKey = _apiKey;
                OnPropertyChanged();
            }
        }

        public int Time
        {
            get { return _time; }
            set
            {
                _time = value;
                Settings.Instance.TimeBetweenCalls = _time;
                OnPropertyChanged();
            }
        }

        public ICommand SetDefaultValues
        {
            get { return _setDefaultValues; }
            set
            {
                _setDefaultValues = value;
                OnPropertyChanged();
            }
        }

        public string SearchField
        {
            get { return _searchField; }
            set
            {
                _searchField = value;
                OnPropertyChanged();
                FilterSearchedResult();
            }
        }

        public ObservableCollection<CurrencyModel> FilteredModels
        {
            get { return _filteredModels; }
            set
            {
                _filteredModels = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region <Methods>
        /// <summary>
        /// Event for button "Save". 
        /// Stores selected currencies in local db.
        /// API key & time are being saved in settings.txt
        /// </summary>
        private void Save()
        {
            //1. Filters all currencies to selected
            var currencyModels = CurrencyModels.Where(x => x.IsSelected);
            var count = currencyModels.Count();
            /*2. If collection's size is bigger than MaxCurrencies, 
              takes first 7 currencies from selected. 
             (CurrentData Tab is full with 7 currencies)*/
            if (count > MaxCurrencies)
            {
                currencyModels = currencyModels.Take(MaxCurrencies).ToArray();
            }
            var uow = UnitOfWork.Instance;
            //3. Saves selected models into local DB.
            lock (uow)
            {
                uow.DeleteCurrencies();
                foreach (var model in currencyModels)
                {
                    if (model.IsSelected)
                    {
                        uow.Add(model.ToCurrency());
                    }
                }
                uow.Save();
            }
            //Refresh currencies in all tabs.
            CurrencyLayerApplication.RefreshModels();
            Settings.Instance.Save();
        }
        /// <summary>
        /// Event for button "Set Default". 
        /// </summary>
        private void SetDefault()
        {
            ApiKey = "";
            Time = 0;
            foreach (var currencyModel in CurrencyModels)
            {
                if (currencyModel.IsSelected)
                {
                    currencyModel.IsSelected = false;
                }
            }
        }
        /// <summary>
        /// Filters data by search substring.
        /// </summary>
        private void FilterSearchedResult()
        {
            if (string.IsNullOrEmpty(_searchField))
            {
                var list = new ObservableCollection<CurrencyModel>(_currencyModels.Where(x=>x.IsSelected));
                foreach (var model in _currencyModels)
                {
                    if (!list.Any(x => x.Code == model.Code))
                    {
                        list.Add(model);
                    }
                }
                FilteredModels = list;
            }
            else
            {
                FilteredModels =
                    new ObservableCollection<CurrencyModel>(_currencyModels.Where(x => x.Name.ToLower().Contains(_searchField) || x.Code.ToLower().Contains(_searchField)));
            }
        }

        private void InitCheckBoxs()
        {
            var list = CurrencyLayerApplication.CurrencyModels;
            foreach (var model in list)
            {
                CurrencyModels.First(x => x.Code == model.Code).IsSelected = model.IsSelected;
            }
            FilterSearchedResult();
        }

        #endregion

        #region <Additional>

        protected override void ThreadMethod()
        {
            //ignored
        }

        #endregion


    }
}
