using System;
using System.Windows.Input;

namespace AnterealTest.ViewModels.Base
{
    public class RelayCommand : ICommand
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        /// <summary>
        /// Событие извещающее об измении состояния команды
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Конструктор команды
        /// </summary>
        /// <param name="execute">Выполняемый метод команды</param>
        /// <param name="canExecute">Метод разрешающий выполнение команды</param>
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        /// <summary>
        /// Вызов разрешающего метода команды
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        /// <returns>True - если выполнение команды разрешено</returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute.Invoke(parameter);
        }

        /// <summary>
        /// Вызов выполняющего метода команды
        /// </summary>
        /// <param name="parameter">Параметр команды</param>
        public void Execute(object parameter)
        {
            execute.Invoke(parameter);
        }
    }
}