using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;

namespace FreeReader
{
    public partial class MainWindowModel : ViewModelBase
    {
        public SettingsModel ReadSettings
        {
            get
            {
                return SettingsManager.Instance.ReadSettings;
            }
        }

        private ICommand _MinimizeCommand;
        public ICommand MinimizeCommand
        {
            get
            {
                if (_MinimizeCommand == null)
                {
                    _MinimizeCommand = new RelayCommand(ExecuteMinimizeCommand);
                }
                return _MinimizeCommand;
            }
        }

        private ICommand _CloseCommand;
        public ICommand CloseCommand
        {
            get
            {
                if (_CloseCommand == null)
                {
                    _CloseCommand = new RelayCommand(ExecuteCloseCommand);
                }
                return _CloseCommand;
            }
        }

        private ICommand _MaximizeCommand;
        public ICommand MaximizeCommand
        {
            get
            {
                if (_MaximizeCommand == null)
                {
                    _MaximizeCommand = new RelayCommand(ExecuteMaximizeCommand, CanExecuteMaximizeCommand);
                }
                return _MaximizeCommand;
            }
        }

        private ICommand _RestoreCommand;
        public ICommand RestoreCommand
        {
            get
            {
                if (_RestoreCommand == null)
                {
                    _RestoreCommand = new RelayCommand(ExecuteRestoreCommand, CanExecuteRestoreCommand);
                }
                return _RestoreCommand;
            }
        }

        private WindowState _windowState;
        public WindowState WindowState
        {
            get { return _windowState; }
            set
            {
                _windowState = value;
                RaisePropertyChanged("WindowState");
            }
        }

        private void ExecuteCloseCommand(object x)
        {
            if (x != null)
            {
                var window = (Window)x;
                SystemCommands.CloseWindow(window);
            }
        }

        private void ExecuteMaximizeCommand(object x)
        {
            if (x != null)
            {
                var window = (Window)x;
                SystemCommands.MaximizeWindow(window);
            }
        }

        private bool CanExecuteMaximizeCommand(object x)
        {
            if (x == null) return false;

            var window = (Window)x;
            return window.WindowState == WindowState.Maximized ? false : true;
        }

        private void ExecuteMinimizeCommand(object x)
        {
            if (x != null)
            {
                var window = (Window)x;
                SystemCommands.MinimizeWindow(window);
            }
        }

        private void ExecuteRestoreCommand(object x)
        {
            if (x != null)
            {
                var window = (Window)x;
                SystemCommands.RestoreWindow(window);
            }
        }

        private bool CanExecuteRestoreCommand(object x)
        {
            if (x == null)
            {
                return false;
            }
            var window = (Window)x;
            return window.WindowState == WindowState.Normal ? false : true;
        }
    }
}
