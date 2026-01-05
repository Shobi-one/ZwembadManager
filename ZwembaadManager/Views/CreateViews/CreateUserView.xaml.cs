using System;
using System.Windows.Controls;
using ZwembaadManager.Events;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateUserView.xaml
    /// </summary>
    public partial class CreateUserView : UserControl
    {
        private readonly CreateUserViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<UserSavedEventArgs>? UserSaveRequested;

        public CreateUserView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateUserViewModel(dataService);

            // Forward ViewModel events to View events for MainWindow
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.UserSaveRequested += (sender, e) => UserSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}