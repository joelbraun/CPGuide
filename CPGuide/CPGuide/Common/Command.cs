using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CPGuide.Common
{
    class Command : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action action;
        private bool canExecute = true;
        public Command(Action action, bool canExecute = true)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            this.action = action;
            this.canExecute = canExecute;
        }
        internal bool CanExecuteInternal
        {
            get
            {
                return this.canExecute;
            }
            set
            {
                if (this.canExecute != value)
                {
                    this.canExecute = value;
                    this.OnCanExecuteChanged(EventArgs.Empty);
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return this.canExecute;
        }
        public void Execute(object parameter)
        {
            this.action();
        }
        private void OnCanExecuteChanged(EventArgs e)
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, e);
            }
        }
    }
    class Command<T> : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private Action<T> action;
        private bool canExecute = true;
        public Command(Action<T> action, bool canExecute = true)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            this.action = action;
            this.canExecute = canExecute;
        }
        internal bool CanExecuteInternal
        {
            get
            {
                return this.canExecute;
            }
            set
            {
                if (this.canExecute != value)
                {
                    this.canExecute = value;
                    this.OnCanExecuteChanged(EventArgs.Empty);
                }
            }
        }
        public bool CanExecute(object parameter)
        {
            return this.canExecute;
        }
        public void Execute(object parameter)
        {
            this.action((T)parameter);
        }
        private void OnCanExecuteChanged(EventArgs e)
        {
            if (this.CanExecuteChanged != null)
            {
                this.CanExecuteChanged(this, e);
            }
        }
    }
}
