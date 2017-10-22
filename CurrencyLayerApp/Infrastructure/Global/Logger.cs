using System;

namespace CurrencyLayerApp.Infrastructure.Global
{

    sealed class Logger
    {
        /// <summary>
        /// Module which save last message about important events. 
        /// (Singleton Pattern)
        /// </summary>
        public Logger()
        {
            _message = new Tuple<string, Color>("", Color.Gray);
        }

        #region <Fields>

        /// <summary>
        /// Message with color (red - error, gray - empty message, green - good news)
        /// </summary>
        private Tuple<string, Color> _message;
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

        /// <summary>
        /// Saves last message
        /// </summary>
        /// <param name="message">text</param>
        /// <param name="color">color</param>
        private void LogMessage(string message, Color color = Color.Gray)
        {
            _message = new Tuple<string, Color>(message, color);
        }
        /// <summary>
        /// Gets last message.
        /// </summary>
        /// <returns></returns>
        private Tuple<string, Color> GetLastLog()
        {
            // ?? - if left part is null, than take right part, else left
            return _message ?? new Tuple<string, Color>("", Color.Gray);
        }

        #endregion

        #region <Static Methods>

        /// <summary>
        /// Saves last message
        /// </summary>
        /// <param name="message">text</param>
        /// <param name="color">color</param>
        public static void SetLogMessage(string message, Color color = Color.Gray)
        {
            Instance.LogMessage(message, color);
        }

        /// <summary>
        /// Gets last message.
        /// </summary>
        /// <returns></returns>
        public static Tuple<string, Color> GetLogMessage()
        {
            return Instance.GetLastLog();
        }

        #endregion
    }
}