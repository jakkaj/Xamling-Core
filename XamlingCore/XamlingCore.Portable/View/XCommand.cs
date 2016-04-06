using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace XamlingCore.Portable.View
{
    public class XCommand<T> : ICommand
        where T : class
    {
        private readonly Action<T> _callbackObject;
        private readonly Action _callback;

        public XCommand(Action callback)
        {
            _callback = callback;
        }
        public XCommand(Action<T> callbackObject)
        {
            _callbackObject = callbackObject;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_callbackObject != null)
            {
                _callbackObject(parameter as T);
            }
            else
            {
                _callback();
            }
        }

        public event EventHandler CanExecuteChanged;
    }

    public class XCommand : ICommand
       
    {
        private readonly Action<object> _callbackObject;
        private readonly Action _callback;

        public XCommand(Action callback)
        {
            _callback = callback;
        }
        public XCommand(Action<object> callbackObject)
        {
            _callbackObject = callbackObject;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_callbackObject != null)
            {
                _callbackObject(parameter);
            }
            else
            {
                _callback();
            }
        }

        public event EventHandler CanExecuteChanged;
    }
}
