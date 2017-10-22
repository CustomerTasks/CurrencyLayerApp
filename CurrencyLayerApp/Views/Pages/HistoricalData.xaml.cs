using System.Windows.Controls;
using CurrencyLayerApp.ViewModels;

namespace CurrencyLayerApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для HistoricalData.xaml
    /// </summary>
    public partial class HistoricalData : Page
    {
        public HistoricalData()
        {
            InitializeComponent();
            DataContext = new HistoricalDataViewModel(AreaSerie);
        }
    }
}
