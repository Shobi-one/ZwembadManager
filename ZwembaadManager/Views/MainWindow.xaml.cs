using System.Windows;
using ZwembaadManager.ViewModels;

namespace ZwembaadManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }
}