using System;
using System.Windows.Input;

namespace BudgetPlanner.App.Commands
{
    /// <summary>
    /// Implementacija ICommand interfejsa za MVVM pattern.
    /// Command pattern - Ponašajni dizajn šablon.
    /// Omogućava binding komandi iz View-a na metode u ViewModel-u.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute;
        
        /// <summary>
        /// Kreira novu RelayCommand instancu.
        /// </summary>
        /// <param name="execute">Akcija koja se izvršava.</param>
        /// <param name="canExecute">Funkcija koja određuje da li komanda može biti izvršena.</param>
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        
        /// <summary>
        /// Događaj koji se pokreće kada se promeni mogućnost izvršavanja komande.
        /// </summary>
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        
        /// <summary>
        /// Određuje da li komanda može biti izvršena.
        /// </summary>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        
        /// <summary>
        /// Izvršava komandu.
        /// </summary>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
        
        /// <summary>
        /// Ručno pokreće reevaluaciju CanExecute.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
    
    /// <summary>
    /// Generička verzija RelayCommand koja prihvata specifičan tip parametra.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T?> _execute;
        private readonly Predicate<T?>? _canExecute;
        
        public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }
        
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute((T?)parameter);
        }
        
        public void Execute(object? parameter)
        {
            _execute((T?)parameter);
        }
        
        public void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
