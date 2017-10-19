using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLayerApp.Infrastructure
{
    public  static class CommonData
    {
        public static string MainDirectory { get;} = Environment.CurrentDirectory.Replace(@"bin\Debug", "");
        public static string IconFolder = $@"{MainDirectory}\Resources\Pictures\Flags\";

        public static readonly string CurrenciesAssetFile = $@"{MainDirectory}\Resources\Currencies.txt";
        public static readonly string SelectedModelsFile = $@"{MainDirectory}\App_Data\Models.txt";
    }
}
