using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RetiraTracker.Core.Abstracts
{
    public abstract class BaseNotifyPropertyChanged:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool SetValue<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            NotifyPropertyChanged(propertyName);
            return true;
        }
    }
}
