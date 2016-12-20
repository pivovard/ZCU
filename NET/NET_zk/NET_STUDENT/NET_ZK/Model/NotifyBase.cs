using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NET_ZK.Model
{
    class NotifyBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void SetIfChanged<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (property == null || !property.Equals(value))
            {
                property = value;
                RaisePropertyChanged(propertyName);
            }
        }
    }
}
