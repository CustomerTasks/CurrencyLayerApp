namespace CurrencyLayerApp.Abstractions
{
    /// <summary>
    /// Interface for initialization data/models with blocking resources 
    /// (for example: set IsEnabled in UI)
    /// </summary>
    internal interface IInitializationManager
    {
        /// <summary>
        /// Property for blocking UI or etc
        /// </summary>
        bool IsEnabled { get; set; }
        void Initialize();
    }
}