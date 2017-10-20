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
        [Key, Index("Code", IsClustered = false, IsUnique = true)]
        public string Code { get; set; }
        [Required]
        public double Rating { get; set; } = 0;
        public string Name { get; set; }
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; } = new HashSet<HistoricalData>();
    }
}
