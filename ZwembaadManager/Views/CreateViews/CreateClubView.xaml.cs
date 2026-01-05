using System;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Events;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateClubView.xaml
    /// </summary>
    public partial class CreateClubView : UserControl
    {
        private readonly CreateClubViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<ClubSavedEventArgs>? ClubSaveRequested;

        public CreateClubView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateClubViewModel(dataService);

            // Forward ViewModel events to View events for MainWindow
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.ClubSaveRequested += (sender, e) => ClubSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}