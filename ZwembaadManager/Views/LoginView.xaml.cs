using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public event EventHandler<LoginEventArgs>? LoginSuccessful;

        public LoginView()
        {
            InitializeComponent();
            Loaded += (s, e) => txtUsername.Focus();
            KeyDown += LoginView_KeyDown;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            PerformLogin();
        }

        private void LoginView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PerformLogin();
            }
        }

        private void PerformLogin()
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username))
            {
                MessageBox.Show("Gelieve een gebruikersnaam in te voeren.", "Validatie", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Gelieve een wachtwoord in te voeren.", "Validatie", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPassword.Focus();
                return;
            }

            LoginSuccessful?.Invoke(this, new LoginEventArgs { Username = username });
        }

        public void ClearCredentials()
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }

    public class LoginEventArgs : EventArgs
    {
        public string Username { get; set; } = string.Empty;
    }
}