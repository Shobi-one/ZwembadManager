using System;
using System.Windows.Controls;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateUsersFunctionView.xaml
    /// </summary>
    public partial class CreateUsersFunctionView : UserControl
    {
        private readonly CreateUsersFunctionViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? UsersFunctionSaveRequested;

        public CreateUsersFunctionView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateUsersFunctionViewModel(dataService);

            // Forward ViewModel events to View events for MainWindow
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.UsersFunctionSaveRequested += (sender, e) => UsersFunctionSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}