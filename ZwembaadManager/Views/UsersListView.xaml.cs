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
            DataContext = new UsersListViewModel(dataService);
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}