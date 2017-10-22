using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLayerApp.DAL.Entities
{
    /// <summary>
    /// Entity Currency
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Code (USD, ILS, EUR ...)
        /// </summary>
        [Key, Index("Code", IsClustered = false, IsUnique = true)]
        public string Code { get; set; }

        /// <summary>
        /// Average rating from last update (CurrencyData tab)
        /// </summary>
        [Required]
        public double Rating { get; set; } = 0;

        /// <summary>
        /// Just Full name of currency
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Each Currency has history data by some days
        /// </summary>
        public virtual ICollection<HistoricalData> HistoricalDatas { get; set; } = new HashSet<HistoricalData>();
    }
}
