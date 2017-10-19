using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CurrencyLayerApp.DAL.Entities;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Helpers
{
    static class ClassConverters
    {
        public static Currency ToCurrency(this CurrencyModel model)=> new  Currency {Code = model.Code,Name = model.Name};
    }
}
