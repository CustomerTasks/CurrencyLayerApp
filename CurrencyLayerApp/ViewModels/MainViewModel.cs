using System;
using System.Threading;
using System.Windows.Input;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure;
using CurrencyLayerApp.Infrastructure.Global;
using CurrencyLayerApp.Resources.Strings;

namespace CurrencyLayerApp.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// ViewModel for MainWindow.xaml
    /// </summary>
    class MainViewModel : ViewModelBase, IInitializationManager
    {
        public MainViewModel()
        {
            Color = Logger.Color.Gray;
        }

        #region <Fields>
        /// <summary>
        /// Current log message. 
        /// (Message is located at bottom-right corner)
        /// </summary>
        private string _logMessage;
        /// <summary>
        /// Current log color. 
        /// (Color element is located at bottom-right corner)
        /// </summary>
        private Logger.Color _color;
        /// <summary>
        /// Index of selected tab.
        /// </summary>
        private int _index;

        private bool _isEnabled;

        #endregion

        #region <Properties>
        

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

        public int Index
        {
            get { return _index; }
            set
            {
                _index = value;
                OnPropertyChanged();
            }
        }
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set { _isEnabled = value; OnPropertyChanged(); }
        }

        #endregion

        #region <Methods>
        /// <summary>
        /// Executes some task in the thread.
        /// Just notifies us about Application work.
        /// </summary>
        protected override void Execute()
        {
            while (true)
            {
                if (Settings.Instance.IsFihished)
                {
                    DownloaderThread.Abort();
                    break;
                }
                Initialize();
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
                CurrencyLayerApplication.ThreadSleep(1);
            }
        }

        public void Initialize()
        {
            Index = Settings.Instance.IsConfigured ? Index : 3;
            if (Index == 3 && !Settings.Instance.IsConfigured)
            {
                Logger.SetLogMessage(MainLogMessages.ApiKeyRequire, Logger.Color.Red);
                IsEnabled = false;
            }
            else
            {
                IsEnabled = true;
            }
        }
        #endregion

        
    }
}
