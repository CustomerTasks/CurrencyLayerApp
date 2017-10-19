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
    public class CurrencyLayerContext: DbContext
    {
        public virtual DbSet<Currency> Currencies { get; set; }

        public CurrencyLayerContext() : base("StandartConnection")
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory().Replace(@"bin\Debug",""));
        }
        public static CurrencyLayerContext Create()
        {
            return new CurrencyLayerContext();
        }
    }
}
