using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZwembaadManager.Views;

namespace ZwembaadManager
{
    public partial class MainWindow : Window
    {
        private LoginView? loginView;
        private DashboardView? dashboardView;

        public MainWindow()
        {
            InitializeComponent();
            ShowLoginView();
        }

        private void ShowLoginView()
        {
            loginView = new LoginView();
            loginView.LoginSuccessful += LoginView_LoginSuccessful;
            MainContentArea.Content = loginView;
        }

        private void LoginView_LoginSuccessful(object? sender, LoginEventArgs e)
        {
            ShowDashboard();
        }

        private void ShowDashboard()
        {
            dashboardView = new DashboardView();
            dashboardView.CreateEntityRequested += Dashboard_CreateEntityRequested;
            dashboardView.LogoutRequested += Dashboard_LogoutRequested;
            dashboardView.RefreshRequested += Dashboard_RefreshRequested;

            MainContentArea.Content = dashboardView;
        }

        private void Dashboard_CreateEntityRequested(object? sender, string entityType)
        {
            switch (entityType)
            {
                case "User":
                    // TODO: Show UserCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "Club":
                    // TODO: Show ClubCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "Meet":
                    // TODO: Show MeetCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "SwimmingPool":
                    // TODO: Show SwimmingPoolCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "Address":
                    // TODO: Show AddressCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "Function":
                    // TODO: Show FunctionCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "FunctionAssignment":
                    // TODO: Show FunctionAssignmentCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "UsersFunction":
                    // TODO: Show UsersFunctionCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "MeetFunction":
                    // TODO: Show MeetFunctionCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                case "JurysMember":
                    // TODO: Show JurysMemberCreateView
                    MessageBox.Show($"Create {entityType} view will be implemented soon.");
                    break;
                default:
                    MessageBox.Show($"Unknown entity type: {entityType}");
                    break;
            }
        }

        private void Dashboard_LogoutRequested(object? sender, EventArgs e)
        {
            ShowLoginView();
        }

        private void Dashboard_RefreshRequested(object? sender, EventArgs e)
        {
            MessageBox.Show("Data refresh functionality will be implemented when JSON data loading is added.");
        }
    }
}