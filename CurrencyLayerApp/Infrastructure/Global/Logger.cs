using System;

namespace CurrencyLayerApp.Infrastructure.Global
{
    sealed class Logger
    {
      
        public Logger()
        {
            _message = new Tuple<string, Color >("", Color.Gray);
        }

        #region >Fields>

        private Tuple<string,Color> _message;
        static Lazy<Logger> _lazy = new Lazy<Logger>(() => new Logger());

        #endregion

        #region <Properties>

        public static Logger Instance { get; } = _lazy.Value;

        #endregion

        #region <Enumerations>

        internal enum Color
        {
            Red,
            Green,
            Gray
        }

        #endregion

        #region <Methods>

        public void LogMessage(string message, Color color = Color.Gray)
        {
            _message = new Tuple<string, Color>(message,color);
        }

        private Tuple<string, Color> GetLastLog() => _message ?? new Tuple<string, Color>("",Color.Gray);
        
        #endregion

        #region <Static Methods>

        public static void SetLogMessage(string message,Color color = Color.Gray)
        {
            Instance.LogMessage(message,color);
        }
        public static Tuple<string, Color> GetLogMessage()
        {
            return Instance.GetLastLog();
        }

        #endregion
    }
}