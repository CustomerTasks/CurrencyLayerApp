using System;
using System.Threading;
using System.Windows.Input;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.DAL.Infrastructure;
using CurrencyLayerApp.Helpers;
using CurrencyLayerApp.Infrastructure.Global;

namespace CurrencyLayerApp.ViewModels
{
    /// <inheritdoc />
    /// <summary>
    /// ViewModel for MainWindow.xaml
    /// </summary>
    class MainViewModel : ViewModelBase
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
                    Thread.Abort();
                    break;
                }
                Index = Settings.Instance.IsConfigured ? Index : 3;
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
