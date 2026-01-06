using System;
using System.Windows.Controls;
using ZwembaadManager.Events;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateMeetView.xaml
    /// </summary>
    public partial class CreateMeetView : UserControl
    {
        private readonly CreateMeetViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<MeetSavedEventArgs>? MeetSaveRequested;

        public CreateMeetView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateMeetViewModel(dataService);
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.MeetSaveRequested += (sender, e) => MeetSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}