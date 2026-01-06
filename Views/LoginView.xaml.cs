using System;
using System.Windows.Controls;
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
            _viewModel.LoginSuccessful += (sender, e) => LoginSuccessful?.Invoke(this, e);

            DataContext = _viewModel;

            Loaded += (s, e) => txtUsername.Focus();
        }

        public void ClearCredentials()
        {
            _viewModel.ClearCredentials();
            txtPassword.Clear();
            txtUsername.Focus();
        }
    }
}