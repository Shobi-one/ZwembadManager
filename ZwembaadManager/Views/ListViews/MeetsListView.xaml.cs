using System;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Models;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    public partial class MeetsListView : UserControl
    {
        public event EventHandler? BackRequested;

        public MeetsListView()
        {
            InitializeComponent();
            var dataService = new JsonDataService();
            var viewModel = new MeetsListViewModel(dataService);
            DataContext = viewModel;

            Loaded += async (s, e) =>
            {
                if (viewModel.LoadMeetsCommand.CanExecute(null))
                {
                    viewModel.LoadMeetsCommand.Execute(null);
                }
            };
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }

        private void MeetButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Meet meet && DataContext is MeetsListViewModel viewModel)
            {
                viewModel.SelectedMeet = meet;
            }
        }
    }
}