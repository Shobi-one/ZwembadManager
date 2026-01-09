using System;
using System.Windows.Controls;
using ZwembaadManager.Events;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateFunctionView.xaml
    /// </summary>
    public partial class CreateFunctionView : UserControl
    {
        private readonly CreateFunctionViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<FunctionSavedEventArgs>? FunctionSaveRequested;

        public CreateFunctionView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateFunctionViewModel(dataService);
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.FunctionSaveRequested += (sender, e) => FunctionSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}