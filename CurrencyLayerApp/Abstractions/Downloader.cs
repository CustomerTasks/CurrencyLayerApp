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
            DownloaderThread = new Thread(Execute);
            DownloaderThread.Start();
        }

        /// <summary>
        /// Thread with downloading
        /// </summary>
        protected readonly Thread DownloaderThread;
        /// <summary>
        /// Executes some task in the thread.
        /// </summary>
        protected abstract void Execute();
    }
}
