using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MajorExpressWMS.Data
{
    internal class RelayCommand(Action<object?> Execute, Func<object?, bool>? CanExecute = null) : ICommand
    {
        private readonly Action<object?> _Execute = Execute ?? throw new ArgumentNullException(nameof(Execute));
        private readonly Func<object?, bool>? canExecute = CanExecute;

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => canExecute == null || canExecute(parameter);

        public void Execute(object? parameter) => _Execute(parameter);
    }
}