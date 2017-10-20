using System.Windows.Controls;
using CurrencyLayerApp.ViewModels;

namespace CurrencyLayerApp.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для CurrentData.xaml
    /// </summary>
    public partial class CurrentData : Page
    {
        public CurrentData()
        {
            InitializeComponent();
            var context = new CurrentDataViewModel(CurrentDataGrid);
            DataContext = context;
        }
    }
}
