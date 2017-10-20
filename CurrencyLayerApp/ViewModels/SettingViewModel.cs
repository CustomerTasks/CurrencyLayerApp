using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class SettingViewModel : ViewModelBase
    {
        public SettingViewModel()
        {
            CurrencyModels = new ObservableCollection<CurrencyModel>(Parsers.GetStoredModels());
            InitCheckBoxs();
            SaveChanges = new Command(() => Task.Run(() => Save()));
            SetDefaultValues = new Command(SetDefault);
            ApiKey = Settings.Instance.ApiKey;
            Time = Settings.Instance.TimeBetweenCalls;
        }

        #region <Fields>

        private ObservableCollection<CurrencyModel> _currencyModels;
        private ICommand _savechanges;
        private string _apiKey;
        private int _time = 10;
        private ICommand _setDefaultValues;

        #endregion

        #region <Properties>

        public ObservableCollection<CurrencyModel> CurrencyModels
        {
            get { return _currencyModels; }
            set
            {
                _currencyModels = value;
                OnPropertyChanged();
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

        #endregion

        #region <Methods>

        private void Save()
        {
            var uow = UnitOfWork.Instance;
            uow.DeleteCurrencies();
            foreach (var model in CurrencyModels)
            {
                if (model.IsSelected)
                {
                    uow.Add(model.ToCurrency());
                }
            }
            uow.Save();
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

        private void InitCheckBoxs()
        {
            var list = Parsers.GetStoredModels(true);
            foreach (var model in list)
            {
                CurrencyModels.First(x => x.Code == model.Code).IsSelected = model.IsSelected;
            }
        }

        #endregion

        #region <Additional>



        #endregion
    }
}
