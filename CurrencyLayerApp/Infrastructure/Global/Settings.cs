using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CurrencyLayerApp.DAL.Infrastructure;

namespace CurrencyLayerApp.Infrastructure.Global
{
    /// <summary>
    /// Settings, configurations, app data
    /// </summary>
    [Serializable]
    public sealed class Settings
    {
        private Settings()
        {
            Read();
        }

        #region <Fields>

        [NonSerialized]
        private static readonly Lazy<Settings> Lazy = new Lazy<Settings>(() => new Settings());

        #endregion

        #region <Properties>

        public static Settings Instance { get; } = Lazy.Value;
        /// <summary>
        /// User's API key
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Time between API calls
        /// </summary>
        public int TimeBetweenCalls { get; set; }
        /// <summary>
        /// App's State (running/finished)
        /// </summary>
        public bool IsFihished { get; set; }
        /// <summary>
        /// App data state (including API key and configured DB)
        /// </summary>
        public bool IsConfigured { get; set; }

        #endregion

        #region <Methods>
        /// <summary>
        /// Saves API key, time to settings.txt (bin/Debug or Release) using serialization
        /// </summary>
        public void Save()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(CommonData.SettingsFile, FileMode.OpenOrCreate))
            {
                binaryFormatter.Serialize(stream, this);
            }
        }
        /// <summary> 
        /// Reads API key and time from settings.txt  (bin/Debug or Release) using deserialization
        /// </summary>
        public void Read()
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(CommonData.SettingsFile, FileMode.OpenOrCreate))
            {
                if (stream.Length <= 0) return;
                var obj = binaryFormatter.Deserialize(stream);
                var converted = (Settings) obj;
                ApiKey = converted.ApiKey;
                TimeBetweenCalls = converted.TimeBetweenCalls;
            }
        }

        #endregion
    }
}
