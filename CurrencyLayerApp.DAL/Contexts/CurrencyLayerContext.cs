using System;
using System.Data.Entity;
using CurrencyLayerApp.DAL.Entities;


namespace CurrencyLayerApp.DAL.Contexts
{
    /// <summary>
    /// Context for the access to a local DB with name StandartConnection
    /// </summary>
    internal class CurrencyLayerContext : DbContext
    {
        /// <summary>
        /// Table 'Currencies' which store data for Setting & Current Data Tabs
        /// </summary>
        public virtual DbSet<Currency> Currencies { get; set; }

        /// <summary>
        /// Table 'HistoricalDatas' which store data for Historical Data Tab
        /// </summary>
        public virtual DbSet<HistoricalData> HistoricalDatas { get; set; }

        /// <summary>
        /// Context for the access to a local DB with name StandartConnection
        /// </summary>
        public CurrencyLayerContext() : base("StandartConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory",
                System.IO.Directory.GetCurrentDirectory().Replace(@"bin\Debug", ""));
        }
    }
}
