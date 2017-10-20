using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using CurrencyLayerApp.DAL.Entities;


namespace CurrencyLayerApp.DAL.Contexts
{
    internal class CurrencyLayerContext: DbContext
    {
        public virtual DbSet<Currency> Currencies { get; set; }
        public virtual DbSet<HistoricalData> HistoricalDatas { get; set; }

        public CurrencyLayerContext() : base("StandartConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory().Replace(@"bin\Debug",""));
        }
    }
}
