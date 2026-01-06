using System;
using System.Windows.Controls;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateMeetFunctionView.xaml
    /// </summary>
    public partial class CreateMeetFunctionView : UserControl
    {
        private readonly CreateMeetFunctionViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? MeetFunctionSaveRequested;

        public CreateMeetFunctionView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateMeetFunctionViewModel(dataService);
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.MeetFunctionSaveRequested += (sender, e) => MeetFunctionSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}