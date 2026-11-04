using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Windows.Threading;

namespace WpfAEmailApp.ViewModels
{
    public class StopwatchViewModel : INotifyPropertyChanged
    {
        private DispatcherTimer _timer;
        private int _secondsElapsed;
        private bool _isRunning;

        public int SecondsElapsed
        {
            get => _secondsElapsed;
            set
            {
                if (_secondsElapsed != value)
                {
                    _secondsElapsed = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SecondAngle));
                    OnPropertyChanged(nameof(MinuteAngle));
                }
            }
        }

        
        public double SecondAngle => (SecondsElapsed % 60) * 6;

        
        public double MinuteAngle => ((SecondsElapsed / 60) % 60) * 6;

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand ResetCommand { get; }

        public StopwatchViewModel()
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += (s, e) => SecondsElapsed++;

            StartCommand = new RelayCommand(Start, () => !_isRunning);
            StopCommand = new RelayCommand(Stop, () => _isRunning);
            ResetCommand = new RelayCommand(Reset, () => SecondsElapsed > 0 && !_isRunning);
        }

        private void Start()
        {
            _timer.Start();
            _isRunning = true;
            CommandManager.InvalidateRequerySuggested();
        }

        private void Stop()
        {
            _timer.Stop();
            _isRunning = false;
            CommandManager.InvalidateRequerySuggested();
        }

        private void Reset()
        {
            _timer.Stop();
            SecondsElapsed = 0;
            _isRunning = false;
            CommandManager.InvalidateRequerySuggested();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}