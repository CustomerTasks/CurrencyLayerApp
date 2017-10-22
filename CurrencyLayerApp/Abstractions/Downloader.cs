using System.Threading;

namespace CurrencyLayerApp.Abstractions
{
    /// <summary>
    /// Interface which provides methods for executing some tasks in the Thread
    /// </summary>
    abstract class Downloader
    {
        protected Downloader()
        {
            Thread = new Thread(Execute);
            Thread.Start();
        }

        /// <summary>
        /// Thread
        /// </summary>
        protected readonly Thread Thread;

        /// <summary>
        /// Executes some task in the thread.
        /// </summary>
        protected abstract void Execute();
    }
}
