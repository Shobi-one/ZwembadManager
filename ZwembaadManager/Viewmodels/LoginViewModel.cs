using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Events;

namespace ZwembaadManager.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username = string.Empty;
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
            LoginCommand = new RelayCommand(() => PerformLogin());
        }

        public bool ValidateAndLogin(string username, string password)
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Gelieve een gebruikersnaam in te voeren.", "Validatie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Gelieve een wachtwoord in te voeren.", "Validatie", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            Username = username.Trim();
            LoginSuccessful?.Invoke(this, new LoginEventArgs { Username = Username });
            return true;
        }

        private void PerformLogin()
        {
            // This method will be called from the view with password parameter
            // Password binding is handled differently due to security
        }

        public void ClearCredentials()
        {
            Username = string.Empty;
            ErrorMessage = string.Empty;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}