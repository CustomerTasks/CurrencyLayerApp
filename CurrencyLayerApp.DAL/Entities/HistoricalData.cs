using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyLayerApp.DAL.Entities
{
    /// <summary>
    /// Entity HistoricalData
    /// </summary>
    public class HistoricalData
    {
        [Key]
        public int Id { get; set; }
        /// <summary>
        /// Date of average evaluation
        /// </summary>
        [Required]
        public DateTime? Date { get; set; }
        /// <summary>
        /// Average evaluated rating by fixed date
        /// </summary>
        [Required]
        public double Rating { get; set; }
        /// <summary>
        /// Currency which has this data.
        /// </summary>
        [Required]
        public virtual Currency Currency { get; set; }
    }
}