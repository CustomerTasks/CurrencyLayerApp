using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLayerApp.Models
{
    interface ICurrency
    {
         string Code { get; set; }
    }
    public class CurrencyModel: ICurrency
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; } = false;
        public string Code { get; set; }
        public double Rating { get; set; }
    }

    class CurrencyRate : ICurrency
    {
        public string Code { get; set; }
        public double Rate { get; set; }
    }
}
