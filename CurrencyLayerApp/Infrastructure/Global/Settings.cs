﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace CurrencyLayerApp.Infrastructure.Global
{
    [Serializable]
    public sealed class Settings
    {
        private Settings() {Read(); }
        [NonSerialized]
        private static readonly Lazy<Settings> Lazy = new Lazy<Settings>(() => new Settings());
        public static Settings Instance { get; } = Lazy.Value;

        public string ApiKey { get; set; }
        public int TimeBetweenCalls { get; set; }

        public void Save()
        { BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream stream= new FileStream(CommonData.SettingsFile,FileMode.OpenOrCreate))
            {
               
                binaryFormatter.Serialize(stream,this);
            }
        }

        public void Read()
        {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream stream = new FileStream(CommonData.SettingsFile, FileMode.OpenOrCreate))
            {
                if(stream.Length<=0) return;
                var obj=binaryFormatter.Deserialize(stream);
                var converted = (Settings) obj;
                ApiKey = converted.ApiKey;
                TimeBetweenCalls = converted.TimeBetweenCalls;
            }
        }
    }
}