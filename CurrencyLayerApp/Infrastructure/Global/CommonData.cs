﻿using System;
using System.Linq;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Models;

namespace CurrencyLayerApp.Infrastructure.Global
{
    public static class CommonData
    {

        #region Local Paths

        public static string MainDirectory { get; } = Environment.CurrentDirectory.Replace(@"bin\Debug", "");
        public static string SettingsFile { get; } = "settings.txt";

        public static string IconFolder = $@"{MainDirectory}\Resources\Pictures\Flags\";

        public static readonly string CurrenciesAssetFile = $@"{MainDirectory}\Resources\Currencies.txt";
        public static readonly string SelectedModelsFile = $@"{MainDirectory}\App_Data\Models.txt";

        #endregion

        #region Api Adresses

        public static readonly string CurrentLayerApi = "http://apilayer.net/api/";
        public static readonly string CurrentLayerApiLiveData = "live";
        public static readonly string CurrentLayerApiHistoricalData = "historical";

        #endregion

        public static class MainLogMessages
        {
            public static readonly string NotAvailableInternetMessage =
                "Internet isn`t available. Please, check connection";

            public static readonly string ConnectedMessage = "Connected to CurrentLayerServer";
            public static readonly string StartupMessage = "Application isn`t prepared for working";
        }
    }
}
