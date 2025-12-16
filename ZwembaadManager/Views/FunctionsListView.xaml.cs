using System;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    public partial class FunctionsListView : UserControl
    {
        public event EventHandler? BackRequested;

        public FunctionsListView()
        {
            InitializeComponent();
            var dataService = new JsonDataService();
            DataContext = new FunctionsListViewModel(dataService);
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}