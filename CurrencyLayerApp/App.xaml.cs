using System.Windows;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Infrastructure.Global;

namespace CurrencyLayerApp
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnExit(ExitEventArgs e)
        {
            UnitOfWork.Instance.Dispose();
            Settings.Instance.IsFihished = true;
            base.OnExit(e);
        }
    }
}
