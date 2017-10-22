namespace CurrencyLayerApp.Abstractions
{
    /// <summary>
    /// Currency base interface
    /// </summary>
    interface ICurrency
    {
        /// <summary>
        /// Code
        /// </summary>
        string Code { get; set; }
    }
}