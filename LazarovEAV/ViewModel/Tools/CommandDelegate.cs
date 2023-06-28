using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace LazarovEAV.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class CommandDelegateBase<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private bool canExecute = true;
        public bool IsEnabled { get { return this.canExecute; } set { this.canExecute = value; if (this.CanExecuteChanged != null) this.CanExecuteChanged(this, new EventArgs()); } }

        private Action<T> _executeDelegate;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="executeDelegate"></param>
        public CommandDelegateBase(Action<T> executeDelegate) 
        {
            _executeDelegate = executeDelegate;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _executeDelegate((T)parameter);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter) 
        { 
            return true; 
        }
    }


    /// <summary>
    /// 
    /// </summary>
    public class CommandDelegate : CommandDelegateBase<object>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="executeDelegate"></param>
        public CommandDelegate(Action<object> executeDelegate)
            : base(executeDelegate)
        {
        }
    }
}
