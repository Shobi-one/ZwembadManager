using System;
using System.Windows.Controls;
using ZwembaadManager.Services;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateJurysMemberView.xaml
    /// </summary>
    public partial class CreateJurysMemberView : UserControl
    {
        private readonly CreateJurysMemberViewModel _viewModel;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? JurysMemberSaveRequested;

        public CreateJurysMemberView()
        {
            InitializeComponent();

            var dataService = new JsonDataService();
            _viewModel = new CreateJurysMemberViewModel(dataService);
            _viewModel.BackToDashboardRequested += (sender, e) => BackToDashboardRequested?.Invoke(this, e);
            _viewModel.JurysMemberSaveRequested += (sender, e) => JurysMemberSaveRequested?.Invoke(this, e);

            DataContext = _viewModel;
        }
    }
}