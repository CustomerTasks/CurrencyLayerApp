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
            CurrencyModels = new ObservableCollection<CurrencyModel>(Parsers.ParseCurrencyModels());
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
