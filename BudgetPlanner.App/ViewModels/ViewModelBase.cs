using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BudgetPlanner.App.ViewModels
{
    /// <summary>
    /// Bazna klasa za sve ViewModele.
    /// Implementira INotifyPropertyChanged za data binding.
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Statički event koji se poziva kada se dodaje/ažurira/briše transakcija.
        /// MainViewModel se subscribe-uje na ovaj event da osvežava brojače.
        /// </summary>
        public static event Action? TransactionChanged;
        
        /// <summary>
        /// Pokreće PropertyChanged događaj.
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        
        /// <summary>
        /// Postavlja vrednost property-ja i pokreće PropertyChanged ako se vrednost promenila.
        /// </summary>
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// Poziva TransactionChanged event.
        /// Koristi se nakon dodavanja/ažuriranja/brisanja transakcije.
        /// </summary>
        protected static void RaiseTransactionChanged()
        {
            TransactionChanged?.Invoke();
        }
    }
}
