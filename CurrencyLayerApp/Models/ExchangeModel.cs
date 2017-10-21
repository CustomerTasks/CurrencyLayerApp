namespace CurrencyLayerApp.Models
{
        public class ExchangeModel:ICurrency
        {
            public string Code { get; set; }
            public double Value { get; set; }
        }
}
