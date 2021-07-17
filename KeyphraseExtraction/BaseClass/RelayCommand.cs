using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace KeyphraseExtraction.BaseClass
{
    // <summary>
    /// A command(with T type) whose sole a purpose is to replay it functionality
    /// to other object by invoking delegates. The default return value
    /// for the CanExecute method is 'true'
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
         #region Fields
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;
        #endregion

        #region Constructor
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute is null");
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion

        #region ICommand Members
        /// <summary>
        /// This method can determine the command is executed or not.
        /// The return value of this method (boolean) will affect 
        /// the Enabled property of the control related to the command.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute((T)parameter);
        }
        /// <summary>
        /// This method will be executed when the command is activated
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
        /// <summary>
        /// This event triggered every time there is a change
        /// affecting the command. The control uses command will 
        /// enable / disable depending on the results returned by CanExecute() method.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }
        #endregion
    }

    /// <summary>
    /// A command(with T type) whose sole a purpose is to replay it functionality
    /// to other object by invoking delegates. The default return value
    /// for the CanExecute method is 'true'
    /// </summary>
    public class RelayCommands : ObservableNotifyObject,ICommand
    {
        #region Fields
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;
        private Key _key;
        private ModifierKeys _modifiers;

        #endregion

        #region Constructor
        /// <summary>
        /// Create replay command with action
        /// </summary>
        /// <param name="execute"></param>
        public RelayCommands(Action execute)
            : this(execute, null)
        { }
        /// <summary>
        /// Create replay command with action and can execute
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommands(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute is null");
            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Create replay command with action and can execute
        /// </summary>
        /// <param name="execute"></param>
        /// <param name="canExecute"></param>
        public RelayCommands(Action execute, Func<bool> canExecute, Key _key, ModifierKeys _modifiers)
        {
            if (execute == null)
                throw new ArgumentNullException("execute is null");
            _execute = execute;
            _canExecute = canExecute;
            Key = _key;
            Modifiers = _modifiers;
        }
        #endregion

        #region ICommand Members
        /// <summary>
        /// This method can determine the command is executed or not.
        /// The return value of this method (boolean) will affect 
        /// the Enabled property of the control related to the command.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute();
        }
        /// <summary>
        /// This method will be executed when the command is activated
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            _execute();
        }
        /// <summary>
        /// This event triggered every time there is a change
        /// affecting the command. The control uses command will 
        /// enable / disable depending on the results returned by CanExecute() method.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }
        #endregion

        public Key Key
        {
            get { return _key; }
            set
            {
                _key = value;
                RaisePropertyChanged("Key");
                RaisePropertyChanged("Text");
            }
        }

        public ModifierKeys Modifiers
        {
            get
            { return _modifiers; }
            set
            {
                _modifiers = value;
                RaisePropertyChanged("Modifiers");
                RaisePropertyChanged("Text");
            }
        }

        private static Dictionary<ModifierKeys, string> modifierText = new Dictionary<ModifierKeys, string>()
        {
            {ModifierKeys.None,""},
            {ModifierKeys.Control,"Ctrl+"},
            {ModifierKeys.Control|ModifierKeys.Shift,"Ctrl+Shift+"},
            {ModifierKeys.Control|ModifierKeys.Alt,"Ctrl+Alt+"},
            {ModifierKeys.Control|ModifierKeys.Shift|ModifierKeys.Alt,"Ctrl+Shift+Alt+"},
            {ModifierKeys.Windows,"Windows+"}
        };

        public string Text
        {
            get { return modifierText[_modifiers] + _key.ToString(); }
        }
    }
}
