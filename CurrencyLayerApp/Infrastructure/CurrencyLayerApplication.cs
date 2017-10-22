using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Models;
using CurrencyLayerApp.Resources.Strings;

namespace CurrencyLayerApp.Infrastructure
{
    /// <summary>
    /// Application structure which store in RAM frequently used data
    /// </summary>
    internal static class CurrencyLayerApplication
    {
        /// <summary>
        /// General selected currencies.
        /// </summary>
        public static CurrencyModel[] CurrencyModels;

        static CurrencyLayerApplication()
        {
            RefreshModels();
            //Checks is application prepared for using
            if (CurrencyModels == null || !CurrencyModels.Any() || string.IsNullOrEmpty(Settings.Instance.ApiKey))
            {
                Settings.Instance.IsConfigured = false;
                Logger.SetLogMessage(MainLogMessages.StartupMessage, Logger.Color.Red);
            }
            else
            {
                Settings.Instance.IsConfigured = true;
            }
        }
        /// <summary>
        /// Reread currencies from DB
        /// </summary>
        public static void RefreshModels()
        {
            CurrencyModels = Parsers.GetStoredModels(true);
            Settings.Instance.IsConfigured = true;
        }
        /// <summary>
        /// Sets current thread in sleeping mode for time between calls.
        /// </summary>
        public static void ThreadSleep()
        {
            Thread.Sleep(Settings.Instance.TimeBetweenCalls * 1000);
        }
    }
}
