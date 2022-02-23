using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace techsolaWorkClockTimer
{
    public abstract record ObservableRecord : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool Set<T>(ref T location, T value, [CallerMemberName] string? propertyName = null)
        {
            if (RuntimeHelpers.Equals(location, value)) return false;
            location = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
