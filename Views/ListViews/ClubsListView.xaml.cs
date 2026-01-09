using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    public partial class ClubsListView : UserControl
    {
        public event EventHandler? BackRequested;

        public ClubsListView()
        {
            InitializeComponent();
            var dataService = new JsonDataService();
            DataContext = new ClubsListViewModel(dataService);
        }

        private void BackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}