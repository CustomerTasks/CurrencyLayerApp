using System;
using System.Threading;
using System.Windows.Input;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure.Global;

namespace CurrencyLayerApp.ViewModels
{
    class MainViewModel : ViewModelBase, IDownloader
    {
        public MainViewModel()
        {
            Color = Logger.Color.Gray;
            Thread = new Thread(DownloadData);
            Thread.Start();
        }



        #region <Fields>

        private string _logMessage;
        private Logger.Color _color;
        private bool _isOnline;

        #endregion

        #region <Properties>

        public Thread Thread { get; set; }

        public string LogMessage
        {
            get { return _logMessage; }
            set
            {
                _logMessage = value;
                OnPropertyChanged();
            }
        }

        public Logger.Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged();
            }
        }

       

        #endregion

        #region <Methods>

        public static void Close()
        {
            UnitOfWork.Instance.Dispose();
            GC.Collect();
        }

        public void DownloadData()
        {
            while (true)
            {
                var log = Logger.GetLogMessage();
                if (log != null)
                {
                    LogMessage = log.Item1;
                    Color = log.Item2;
                }
                else
                {
                    LogMessage = "";
                    Color = Logger.Color.Gray;
                }
                Thread.Sleep(1000);
            }
        }

        #endregion
    }
}
