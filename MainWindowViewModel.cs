#define OBSERVABLE_COLLECTION

using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WpfUiThread
{
    public class MainWindowViewModel: INotifyPropertyChanged
    {
        public MainWindowViewModel()
        {
            TimeElapsed.Add("Seed");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private string _message = null;
        private string _errorMessage;
        private bool _progressBarRunning = false;

#if OBSERVABLE_COLLECTION
        private ObservableCollection<string> _timeElapsed = new ObservableCollection<string>();
#else
        private BindableCollection<string> _timeElapsed = new BindableCollection<string>();
#endif

        public IList<string> TimeElapsed
        {
            get { return _timeElapsed; }
        }
        
        public string Message
        {
            get { return _message; }
            set { SetAndRaiseChanged(ref _message, value); }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set {  SetAndRaiseChanged(ref _errorMessage, value); }
        }

        public bool ProgressBarRunning
        {
            get { return _progressBarRunning; }
            set { SetAndRaiseChanged(ref _progressBarRunning, value); }
        }
        
        private void SetAndRaiseChanged<T>(ref T valueStore, T value, [CallerMemberName] string propertyName = null)
        {
            if (!value.Equals(valueStore)) {
                valueStore = value;
                var propChanged = PropertyChanged;
                if (propChanged != null) {
                    propChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }
    
        public void UpdateFreezeButton()
        {
            UpdateDatabase();
            Message = "Database updated.";
        }

        public void UpdateThreadButton()
        {
            var threadStart = new ThreadStart(delegate
            {
                ErrorMessage = "";
                UpdateDatabase();
                Message = "Database updated";
            });

            new Thread(threadStart).Start();
            
        }

        public void UpdateDatabase()
        {
            ErrorMessage = "";
            ProgressBarRunning = true;
            Thread.Sleep(2500);
            _timeElapsed.Add("Test");
            ProgressBarRunning = false;
        }

        public void Reset()
        {
            Message = "";
        }

        
    }
}
