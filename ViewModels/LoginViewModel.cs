using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Events;
using ZwembaadManager.Models;

namespace ZwembaadManager.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _password = string.Empty;
        private string _errorMessage = string.Empty;

        public event EventHandler<LoginEventArgs>? LoginSuccessful;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged();
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged();
                    ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                if (_errorMessage != value)
                {
                    _errorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(PerformLogin);
        }

        private void PerformLogin()
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Username))
            {
                MessageBox.Show("Gelieve een gebruikersnaam in te voeren.", "Validatie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Gelieve een wachtwoord in te voeren.", "Validatie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            LoginSuccessful?.Invoke(this, new LoginEventArgs { Username = Username.Trim() });
        }

        public void ClearCredentials()
        {
            Username = string.Empty;
            Password = string.Empty;
            ErrorMessage = string.Empty;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}