using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class SettingViewModel : ViewModelBase
    {
        private ObservableCollection<CurrencyModel> _currencyModels;
        private ICommand _savechanges;

        public SettingViewModel()
        {
            CurrencyModels = GetStoredModels();
            SaveChanges = new Command(Save);
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

        public ICommand SaveChanges
        {
            get { return _savechanges; }
            set
            {
                _savechanges = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<CurrencyModel> GetStoredModels()
        {
            var result = Parsers.ParseCurrencyModels();
            var uow = UnitOfWork.Instance;
            if (uow.Any(typeof(Currency)))
            {
                var models = uow.GetCurrencies();
                foreach (var model in models)
                {
                    if (result.Any(x => x.Code == model.Code))
                    {
                        result.First(x => x.Code == model.Code).IsSelected = true;
                    }
                }
            }
            return new ObservableCollection<CurrencyModel>(result);
        }

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
        }
    }
}
