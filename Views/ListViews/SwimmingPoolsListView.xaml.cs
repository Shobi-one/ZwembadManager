using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Classes.Enum;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    public partial class SwimmingPoolsListView : UserControl
    {
        public event EventHandler? BackRequested;

        public SwimmingPoolsListView()
        {
            InitializeComponent();
            var dataService = new JsonDataService();
            DataContext = new SwimmingPoolsListViewModel(dataService);
            LoadNumberOfLanesOptions();
        }

        private void LoadNumberOfLanesOptions()
        {
            var numberOfLanesOptions = Enum.GetValues<NumberOfLanes>().ToList();
            cmbNumberOfLanes.ItemsSource = numberOfLanesOptions;
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}