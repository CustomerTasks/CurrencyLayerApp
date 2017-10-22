using CurrencyLayerApp.Abstractions;

namespace CurrencyLayerApp.Models
{
    /// <inheritdoc />
    /// <summary>
    /// For converting currencies in Exchange Tab.
    /// </summary>
    public class ExchangeModel : ICurrency
    {
        /// <inheritdoc />
        /// <summary>
        /// Code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Value
        /// </summary>
        public double Rating { get; set; }
    }
}
