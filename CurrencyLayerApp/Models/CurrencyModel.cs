using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyLayerApp.Abstractions;

namespace CurrencyLayerApp.Models
{
    public class CurrencyModel: ICurrency
    {
        /// <summary>
        /// Full name of Code
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Is selected currency in Setting Tab.
        /// </summary>
        public bool IsSelected { get; set; } = false;
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
