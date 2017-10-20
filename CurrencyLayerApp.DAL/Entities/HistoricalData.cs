using System;
using System.ComponentModel.DataAnnotations;

namespace CurrencyLayerApp.DAL.Entities
{
    public class HistoricalData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        [Required]
        public virtual Currency Currency { get; set; }
    }
}