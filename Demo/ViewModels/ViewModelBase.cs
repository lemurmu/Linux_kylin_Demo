using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace QLingScope.ViewModels;

public abstract class ViewModelBase :   INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName) {
        if (this.PropertyChanged != null)
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
    }

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = null) {
        OnPropertyChanged(propertyName);
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(storage, value)) {
            return false;
        }

        storage = value;
        RaisePropertyChanged(propertyName);
        return true;
    }

    protected virtual bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null) {
        if (EqualityComparer<T>.Default.Equals(storage, value)) {
            return false;
        }

        storage = value;
        onChanged?.Invoke();
        RaisePropertyChanged(propertyName);
        return true;
    }
}
