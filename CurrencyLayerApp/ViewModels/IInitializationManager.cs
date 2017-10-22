namespace CurrencyLayerApp.ViewModels
{
    internal interface IInitializationManager
    {
        bool IsEnabled { get; set; }
        void Initialize();
    }
}