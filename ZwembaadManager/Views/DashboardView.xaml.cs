using System;
using System.Windows.Controls;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        public event EventHandler<string>? OpenViewRequested;
        public event EventHandler? LogoutRequested;
        public event EventHandler? RefreshRequested;

        public DashboardView()
        {
            InitializeComponent();
            var viewModel = new DashboardViewModel();

    
            viewModel.OpenViewRequested += (sender, viewType) => OpenViewRequested?.Invoke(this, viewType);
            viewModel.LogoutRequested += (sender, e) => LogoutRequested?.Invoke(this, e);

            DataContext = viewModel;
        }
    }
}