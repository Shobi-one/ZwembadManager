using System;
using System.Windows.Controls;
using ZwembaadManager.Events;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateSwimmingPoolView.xaml
    /// </summary>
    public partial class CreateSwimmingPoolView : UserControl
    {
        private readonly CreateSwimmingPoolViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<SwimmingPoolSavedEventArgs>? SwimmingPoolSaveRequested;

        public CreateSwimmingPoolView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateSwimmingPoolViewModel(dataService);
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.SwimmingPoolSaveRequested += (sender, e) => SwimmingPoolSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}