using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CurrencyLayerApp.Abstractions
{
    internal interface IDownloader
    {
        Thread Thread { get; set; }
        void DownloadData();
    }
}
