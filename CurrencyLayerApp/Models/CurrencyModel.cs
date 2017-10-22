using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyLayerApp.Abstractions;

namespace CurrencyLayerApp.Models
{
    public class CurrencyModel: ExchangeModel
    {
        /// <summary>
        /// Full name of Currency.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Selected currency in Setting Tab.
        /// </summary>
        public bool IsSelected { get; set; } = false;
    }
}
