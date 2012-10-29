using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace WallpaperFlickr.MicroMVVM
{
    public class MicroCommand : ICommand
    {
        public MicroCommand(Func<object, bool> canExecute, Action<object> execute)
        {
            DoCanExecute = canExecute;
            DoExecute = execute;
        }
        
        public Action<object> DoExecute { get; set; }
        public Func<object, bool> DoCanExecute { get; set; }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return DoCanExecute(parameter);
        }

       
        public void Execute(object parameter)
        {
            DoExecute(parameter);
        }
    }
}
