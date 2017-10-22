namespace CurrencyLayerApp.Abstractions
{
    /// <summary>
    /// Interface for initialization data/models with blocking resources 
    /// (for example: set IsEnabled in UI)
    /// </summary>
    internal interface IInitializationManager
    {
        /// <summary>
        /// Property for blocking UI ot etc
        /// </summary>
        bool IsEnabled { get; set; }
        /// <summary>
        /// Initialization
        /// </summary>
        void Initialize();
    }
}