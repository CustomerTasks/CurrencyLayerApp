using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure
{
    internal static class CurrencyLayerApplication
    {
        
        public static CurrencyModel[] CurrencyModels;

        static CurrencyLayerApplication()
        {
            RefreshModels();
            if (CurrencyModels == null || !CurrencyModels.Any() || string.IsNullOrEmpty(Settings.Instance.ApiKey))
            {
                Settings.Instance.IsConfigured = false;
                Logger.SetLogMessage(CommonData.MainLogMessages.StartupMessage, Logger.Color.Red);
            }
            else
            {
                Settings.Instance.IsConfigured = true;
            }
        }

        public static void RefreshModels()
        {
            CurrencyModels = Parsers.GetStoredModels(true);
            Settings.Instance.IsConfigured = true;
        }
        public static void ThreadSleep()
        {
            Thread.Sleep(Settings.Instance.TimeBetweenCalls * 1000);
        }
    }
}
