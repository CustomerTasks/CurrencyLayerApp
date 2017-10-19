using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLayerApp.Models
{
    [Serializable]
    public class CurrencyModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; } = false;
    }
}
