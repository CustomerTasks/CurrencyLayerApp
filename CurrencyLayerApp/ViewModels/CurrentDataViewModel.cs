using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.DataManagers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class CurrentDataViewModel : ViewModelBase, IInitializationManager
    {
        public CurrentDataViewModel(Grid grid):base()
        {
            _grid = grid;
        }

        #region <Fields>

        private ObservableCollection<CurrencyModel> _currencyModels;
        private double[,] _rates;
        private readonly Grid _grid;
        private IDataManager<ApiCurrencyModel> _dataManager;
        private bool _isEnabled;
        ApiCurrencyModel _liveCurrencyModel;

        #endregion

        #region <Properties>

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

        private void InitializeModels()
        {
            _currencyModels = new ObservableCollection<CurrencyModel>(CurrencyLayerApplication.CurrencyModels);
        }

        protected override void ThreadMethod()
        {
            while (true)
            {
                if (Settings.Instance.IsFihished)
                {
                    Thread.Abort();
                    break;
                }
                if (!Settings.Instance.IsConfigured)
                    return;
                Initialize();
                DownloadByManagers();
                if (!IsCreated)
                {
                    Calculation();
                    Task.Run(() => _dataManager.Save(_liveCurrencyModel));
                    _grid.Dispatcher.BeginInvoke((Action) InitializeGrid);
                    IsCreated = true;
                }
                CurrencyLayerApplication.ThreadSleep();
            }
        }


        private void DownloadByManagers()
        {
            _dataManager = new ApiDataManagerForCurrencies(_currencyModels.ToArray());
            var downloaded = _dataManager.Upload();
            if (downloaded != null)
            {
                IsCreated = false;
                _liveCurrencyModel = downloaded;
            }
            if (_liveCurrencyModel == null)
            {
                _dataManager = new LocalDataManagerForCurrencies();
                _liveCurrencyModel = _dataManager.Upload();
            }
            else
            {
                IsCreated = false;
            }
        }

        private void Calculation()
        {
            var dictionary = _liveCurrencyModel.Currencies;
            var size = dictionary.Count;
            _rates = new double[size, size];
            int i = 0, j;
            foreach (var code1 in dictionary)
            {
                j = 0;
                foreach (var code2 in dictionary)
                {
                    _rates[i, j++] = Math.Round(code2.Value / (code1.Value == 0 ? 1 : code1.Value), 5);
                }
                i++;
            }
        }

        #endregion

        #region <Additional>

        private void InitializeGrid()
        {
            if (Settings.Instance.ApiKey == null)
                return;
            Tuple<CurrencyModel, ExchangeModel[]>[] rates =
                new Tuple<CurrencyModel, ExchangeModel[]>[_currencyModels.Count];
            _grid.ColumnDefinitions.Clear();
            _grid.RowDefinitions.Clear();
            for (var i = 0; i <= _currencyModels.Count; i++)
            {
                _grid.ColumnDefinitions.Add(new ColumnDefinition() {Width = new GridLength(1, GridUnitType.Star)});
                _grid.RowDefinitions.Add(new RowDefinition() {Height = new GridLength(1, GridUnitType.Star)});
                if (i < _currencyModels.Count)
                {
                    rates[i] = new Tuple<CurrencyModel, ExchangeModel[]>(_currencyModels[i],
                        new ExchangeModel[_currencyModels.Count]);
                    for (int j = 0; j < rates[i].Item2.Length; j++)
                    {
                        rates[i].Item2[j] = new ExchangeModel {Code = _currencyModels[j].Code, Rating = _rates[i, j]};
                    }
                }
            }

            for (int i = 0; i < rates.Length; i++)
            {
                #region Headers

                var rowheader = GetStackPanel();
                var rowpicturedPanel = GetStackPanel();
                rowpicturedPanel.Orientation = Orientation.Horizontal;
                rowpicturedPanel.Children.Add(new Image() {Source = Helpers.Helpers.GetImage(rates[i].Item1)});
                rowpicturedPanel.Children.Add(GetTextBlock($"1 {rates[i].Item1.Code}", Brushes.Gray));
                rowheader.Children.Add(rowpicturedPanel);
                rowheader.Children.Add(GetTextBlock("Inverse:", Brushes.Gray));
                var colheader = GetStackPanel();
                colheader.Orientation = Orientation.Horizontal;
                colheader.Children.Add(new Image() {Source = Helpers.Helpers.GetImage(rates[i].Item1)});
                colheader.Children.Add(GetTextBlock($"{rates[i].Item1.Code}", Brushes.Gray));

                var panRowHeader = GetPanel(rowheader);
                panRowHeader.SetValue(Grid.ColumnProperty, 0);
                panRowHeader.SetValue(Grid.RowProperty, i + 1);
                var panColHeader = GetPanel(colheader);
                panColHeader.SetValue(Grid.RowProperty, 0);
                panColHeader.SetValue(Grid.ColumnProperty, i + 1);
                _grid.Children.Add(panColHeader);
                _grid.Children.Add(panRowHeader);

                #endregion

                #region Cells

                for (int j = 0; j < rates[i].Item2.Length; j++)
                {
                    var panel = GetStackPanel();
                    panel.Children.Add(GetTextBlock(Math.Round(rates[i].Item2[j].Rating, 5).ToString(), Brushes.Black));
                    if (i != j)
                    {
                        panel.Children.Add(GetTextBlock(Math.Round(rates[j].Item2[i].Rating, 5).ToString(),
                            Brushes.DarkGray));
                    }
                    var panelmain = GetPanel(panel);
                    panelmain.Background = i % 2 != 0 ? Brushes.White : Brushes.LightGray;
                    panelmain.SetValue(Grid.ColumnProperty, j + 1);
                    panelmain.SetValue(Grid.RowProperty, i + 1);
                    _grid.Children.Add(panelmain);
                }
            }

            #endregion
        }

        

        private TextBlock GetTextBlock(string text, SolidColorBrush brush)
        {
            return new TextBlock()
            {
                Text = text,
                Foreground = brush
            };
        }

        private DockPanel GetPanel(params UIElement[] children)
        {
            var panel = new DockPanel();
            foreach (var child in children)
            {
                panel.Children.Add(child);
            }
            return panel;
        }

        private StackPanel GetStackPanel()
        {
            return new StackPanel()
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
        }

        #endregion

       
    }
}