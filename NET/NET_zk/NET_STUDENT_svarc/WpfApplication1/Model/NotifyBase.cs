using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WpfApplication1.Model
{
    /// <summary>
    /// Jednoducha zakladni trida zajistujici implementaci INotifyPropertyChanged.
    /// Lze nahradit nejakou existujicic knihovnou, ktera INPC zajisti napr. nuget PropertyChanged.Fody
    /// </summary>
    public class NotifyBase : INotifyPropertyChanged
    {
        /// <summary>
        /// Udalost pri zmene hodnoty property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Vyvola udalost PropertyChanged
        /// </summary>
        /// <param name="propertyName">Jmeno property</param>
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Metoda porovna stavajici hodnotu property s novou hodnotou a pripadne nastavi novou hodnotu.
        /// Zaroven vyvola udalost PropertyChanged
        /// </summary>
        /// <typeparam name="T">Typ property</typeparam>
        /// <param name="property">Aktualni hodnota property</param>
        /// <param name="value">Nove nastavovana hodnota property</param>
        /// <param name="propertyName">Jmeno property - automaticky pomoci CallerMemberName</param>
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
