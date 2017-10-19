using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLayerApp.DAL.Entities
{
    public class Currency
    {
        [Key,Index("Code",IsClustered = false,IsUnique = true)]
        public string Code { get; set; }
        public string Name { get; set; }
    }
}
