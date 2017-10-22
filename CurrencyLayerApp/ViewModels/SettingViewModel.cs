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
            SaveChanges = new Command(() => Task.Run(() => Save()));
            SetDefaultValues = new Command(SetDefault);
            ApiKey = Settings.Instance.ApiKey;
            Time = Settings.Instance.TimeBetweenCalls;
        }

        #region <Fields>

        private ObservableCollection<CurrencyModel> _currencyModels;
        private ObservableCollection<CurrencyModel> _filteredModels;
        private ICommand _savechanges;
        private string _apiKey;
        private int _time = 10;
        private ICommand _setDefaultValues;
        private string _searchField;
        private const int _maxCurrencies = 7;

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

        private void Save()
        {
            var currencyModels = CurrencyModels.Where(x => x.IsSelected);
            var count = currencyModels.Count();
            if (count > _maxCurrencies)
            {
                currencyModels = currencyModels.Take(_maxCurrencies).ToArray();
            }
            var uow = UnitOfWork.Instance;

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
            CurrencyLayerApplication.RefreshModels();
            Settings.Instance.Save();
        }

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



        #endregion
    }
}
