using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ZwembaadManager.Events;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        private readonly LoginViewModel _viewModel;

        public event EventHandler<LoginEventArgs>? LoginSuccessful;

        public LoginView()
        {
            InitializeComponent();
            _viewModel = new LoginViewModel();

            // Forward ViewModel event to View event for MainWindow
            _viewModel.LoginSuccessful += (sender, e) => LoginSuccessful?.Invoke(this, e);

            DataContext = _viewModel;

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
            // PasswordBox cannot be bound for security reasons, so we handle it in code-behind
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            _viewModel.ValidateAndLogin(username, password);
        }

        public void ClearCredentials()
        {
            _viewModel.ClearCredentials();
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }
}