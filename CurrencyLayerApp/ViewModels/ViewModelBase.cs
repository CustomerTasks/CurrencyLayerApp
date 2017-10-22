using System.ComponentModel;
using System.Runtime.CompilerServices;
using CurrencyLayerApp.Abstractions;
using CurrencyLayerApp.Annotations;

namespace CurrencyLayerApp.ViewModels
{
    /// <summary>
    /// ViewModel Base with implemented 
    /// PropertyChanged notification
    /// </summary>
    abstract class ViewModelBase : Downloader, INotifyPropertyChanged
    {
        /// <summary>
        /// Variable which notifies that the UI is loaded. 
        /// Also need for ignoring operations in threads (savings resource).
        /// </summary>
        public bool IsCreated { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}