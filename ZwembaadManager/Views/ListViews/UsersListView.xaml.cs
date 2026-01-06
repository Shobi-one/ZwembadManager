using System;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    public partial class UsersListView : UserControl
    {
        public event EventHandler? BackRequested;

        public UsersListView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            var dialogService = new DialogService();
            var viewModel = new UsersListViewModel(dataService, dialogService);
            
            DataContext = viewModel;
            
            // Initialize async
            Loaded += async (s, e) => await viewModel.InitializeAsync();
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}