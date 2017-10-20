using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.ViewModels
{
    class CurrentDataViewModel : ViewModelBase
    {
        //private DataView _datatable;
        private ObservableCollection<CurrencyModel> _currencyModels;
        private GridViewColumnCollection _data;
        private double[,] _rates;
        public CurrentDataViewModel()
        {
            _currencyModels =
                new ObservableCollection<CurrencyModel>(Parsers.GetStoredModels(true));
            DownloadData();
        }

        private void DownloadData()
        {
            CurrencyLayerProvider provider = new CurrencyLayerProvider(new HttpClient());
            var liveCurrencyModel = provider.GetLiveCurrencyModel(_currencyModels.ToArray());
            var dictionary = liveCurrencyModel.Quotes;
            var size = dictionary.Count;
            _rates=new double[size,size];
            int i = 0, j=0;
            foreach (var code1 in dictionary)
            {
                j = 0;
                foreach (var code2 in dictionary)
                {
                    _rates[i, j++] = code2.Value / code1.Value;
                }
                i++;
            }
        }

        public void InitializeData(Grid grid)
        {
           
            Tuple<CurrencyModel, CurrencyRate[]>[] rates =
                new Tuple<CurrencyModel, CurrencyRate[]>[_currencyModels.Count];
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            for (var i = 0; i <= _currencyModels.Count; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition(){Width = new GridLength(1,GridUnitType.Star)});
                grid.RowDefinitions.Add(new RowDefinition(){ Height = new GridLength(1, GridUnitType.Star) });
                if (i < _currencyModels.Count)
                {
                    rates[i] = new Tuple<CurrencyModel, CurrencyRate[]>(_currencyModels[i],
                        new CurrencyRate[_currencyModels.Count]);
                    for (int j = 0; j < rates[i].Item2.Length; j++)
                    {
                        rates[i].Item2[j] = new CurrencyRate {Code = _currencyModels[j].Code, Rate = _rates[i,j]};
                    }
                }
            }

            for (int i = 0; i < rates.Length; i++)
            {
                #region Headers

                var rowheader = GetStackPanel();
                var rowpicturedPanel = GetStackPanel();
                rowpicturedPanel.Orientation = Orientation.Horizontal;
                rowpicturedPanel.Children.Add(new Image() {Source = GetImage(rates[i].Item1)});
                rowpicturedPanel.Children.Add(GetTextBlock($"1 {rates[i].Item1.Code}",Brushes.Gray));
                rowheader.Children.Add(rowpicturedPanel);
                rowheader.Children.Add(GetTextBlock("Inverse:", Brushes.Gray));
                var colheader = GetStackPanel();
                colheader.Orientation = Orientation.Horizontal;
                colheader.Children.Add(new Image() { Source = GetImage(rates[i].Item1) });
                colheader.Children.Add(GetTextBlock($"{rates[i].Item1.Code}", Brushes.Gray));

                var panRowHeader = GetPanel(rowheader);
                panRowHeader.SetValue(Grid.ColumnProperty, 0);
                panRowHeader.SetValue(Grid.RowProperty, i + 1);
                var panColHeader = GetPanel(colheader);
                panColHeader.SetValue(Grid.RowProperty, 0);
                panColHeader.SetValue(Grid.ColumnProperty, i + 1);
                grid.Children.Add(panColHeader);
                grid.Children.Add(panRowHeader);

                #endregion


                #region Cells

                for (int j = 0; j < rates[i].Item2.Length; j++)
                {
                    var panel = GetStackPanel();
                    panel.Children.Add(GetTextBlock(Math.Round(rates[i].Item2[j].Rate, 5).ToString(), Brushes.Black));
                    if (i != j)
                    {
                        panel.Children.Add( GetTextBlock (Math.Round(rates[j].Item2[i].Rate, 5).ToString(),Brushes.DarkGray));
                    }
                    var panelmain = GetPanel(panel);
                    panelmain.Background = i % 2 != 0 ? Brushes.White : Brushes.LightGray;
                    panelmain.SetValue(Grid.ColumnProperty, j + 1);
                    panelmain.SetValue(Grid.RowProperty, i + 1);
                    grid.Children.Add(panelmain);
                }
            }

            #endregion
        }

        private ImageSource GetImage(ICurrency currency)
        {
            BitmapImage bimage = new BitmapImage();
            bimage.BeginInit();
            bimage.UriSource =
                new Uri($"{CommonData.IconFolder}{new string(currency.Code.ToLower().Take(2).ToArray())}.png",
                    UriKind.RelativeOrAbsolute);
            bimage.EndInit();
            return bimage;
        }

        private TextBlock GetTextBlock(string text, SolidColorBrush brush)
        {
            return new TextBlock()
            {
               Text = text, Foreground = brush
            };
        }

        private DockPanel GetPanel(params UIElement[] children)
        {
            var panel= new DockPanel();
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

        /*private void FillTable()
        {
            var dataTable=new DataTable();
            dataTable.Columns.Add("#");
            foreach (var model in _currencyModels)
            {
                dataTable.Columns.Add(model.Code);
            }
            foreach (var model in _currencyModels)
            {
                DataRow row = dataTable.NewRow();
                row["#"] = model.Code +"\n Inverse:";
                foreach (var currencyModel in _currencyModels)
                {
                    row[currencyModel.Code] = currencyModel.Code==model.Code?"1\n1": "0\n0";
                }
                dataTable.Rows.Add(row);
            }
            DataTable = dataTable.DefaultView;
        }

        public DataView DataTable
        {
            get { return _datatable; }
            set
            {
                _datatable = value;
                OnPropertyChanged();
            }
        }

        public GridViewColumnCollection Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged();
            }
        }*/
    }
}
