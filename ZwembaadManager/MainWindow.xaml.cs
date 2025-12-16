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
using ZwembaadManager.Events;
using ZwembaadManager.Extensions;

namespace ZwembaadManager
{
    public partial class MainWindow : Window
    {
        private LoginView? loginView;
        private DashboardView? dashboardView;
        private CreateUserView? createUserView;
        private CreateClubView? createClubView;
        private CreateMeetView? createMeetView;
        private CreateSwimmingPoolView? createSwimmingPoolView;
        private CreateFunctionView? createFunctionView;
        private CreateUsersFunctionView? createUsersFunctionView;
        private CreateMeetFunctionView? createMeetFunctionView;
        private CreateJurysMemberView? createJurysMemberView;

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
            dashboardView.OpenViewRequested += Dashboard_OpenViewRequested;
            dashboardView.LogoutRequested += Dashboard_LogoutRequested;
            dashboardView.RefreshRequested += Dashboard_RefreshRequested;

            MainContentArea.Content = dashboardView;
        }

        private void Dashboard_OpenViewRequested(object? sender, string viewType)
        {
            switch (viewType)
            {
                case "CreateUser":
                    ShowCreateUserView();
                    break;
                case "CreateClub":
                    ShowCreateClubView();
                    break;
                case "CreateMeet":
                    ShowCreateMeetView();
                    break;
                case "CreateSwimmingPool":
                    ShowCreateSwimmingPoolView();
                    break;
                case "CreateFunction":
                    ShowCreateFunctionView();
                    break;
                case "CreateUsersFunction":
                    ShowCreateUsersFunctionView();
                    break;
                case "CreateMeetFunction":
                    ShowCreateMeetFunctionView();
                    break;
                case "CreateJurysMember":
                    ShowCreateJurysMemberView();
                    break;
                case "CreateAddress":
                    MessageBox.Show("Create Address view will be implemented soon.");
                    break;
                default:
                    MessageBox.Show($"Unknown view type: {viewType}");
                    break;
            }
        }

        private void ShowCreateUserView()
        {
            createUserView = new CreateUserView();
            createUserView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createUserView.UserSaveRequested += CreateUserView_UserSaveRequested;
            MainContentArea.Content = createUserView;
        }

        private void ShowCreateClubView()
        {
            createClubView = new CreateClubView();
            createClubView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createClubView.ClubSaveRequested += CreateClubView_ClubSaveRequested;
            MainContentArea.Content = createClubView;
        }

        private void ShowCreateMeetView()
        {
            createMeetView = new CreateMeetView();
            createMeetView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createMeetView.MeetSaveRequested += CreateMeetView_MeetSaveRequested;
            MainContentArea.Content = createMeetView;
        }

        private void ShowCreateSwimmingPoolView()
        {
            createSwimmingPoolView = new CreateSwimmingPoolView();
            createSwimmingPoolView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createSwimmingPoolView.SwimmingPoolSaveRequested += CreateSwimmingPoolView_SwimmingPoolSaveRequested;
            MainContentArea.Content = createSwimmingPoolView;
        }

        private void ShowCreateFunctionView()
        {
            createFunctionView = new CreateFunctionView();
            createFunctionView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createFunctionView.FunctionSaveRequested += CreateFunctionView_FunctionSaveRequested;
            MainContentArea.Content = createFunctionView;
        }

        private void ShowCreateUsersFunctionView()
        {
            createUsersFunctionView = new CreateUsersFunctionView();
            createUsersFunctionView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createUsersFunctionView.UsersFunctionSaveRequested += CreateUsersFunctionView_UsersFunctionSaveRequested;
            MainContentArea.Content = createUsersFunctionView;
        }

        private void ShowCreateMeetFunctionView()
        {
            createMeetFunctionView = new CreateMeetFunctionView();
            createMeetFunctionView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createMeetFunctionView.MeetFunctionSaveRequested += CreateMeetFunctionView_MeetFunctionSaveRequested;
            MainContentArea.Content = createMeetFunctionView;
        }

        private void ShowCreateJurysMemberView()
        {
            createJurysMemberView = new CreateJurysMemberView();
            createJurysMemberView.BackToDashboardRequested += CreateView_BackToDashboardRequested;
            createJurysMemberView.JurysMemberSaveRequested += CreateJurysMemberView_JurysMemberSaveRequested;
            MainContentArea.Content = createJurysMemberView;
        }

        private void CreateView_BackToDashboardRequested(object? sender, EventArgs e)
        {
            ShowDashboard();
        }

        private void CreateUserView_UserSaveRequested(object? sender, UserSavedEventArgs e)
        {
            MessageBox.Show($"User '{e.SavedUser.FirstName} {e.SavedUser.LastName}' has been successfully saved to the JSON file!", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateClubView_ClubSaveRequested(object? sender, ClubSavedEventArgs e)
        {
            MessageBox.Show($"Club '{e.SavedClub.Name}' (Abbreviation: '{e.SavedClub.Abbreviation}') has been successfully saved to the JSON file!", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateMeetView_MeetSaveRequested(object? sender, MeetSavedEventArgs e)
        {
            MessageBox.Show($"Meet '{e.SavedMeet.Name}' on {e.SavedMeet.Date:yyyy-MM-dd} ({e.SavedMeet.PartOfTheDay}) has been successfully saved to the JSON file!", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateSwimmingPoolView_SwimmingPoolSaveRequested(object? sender, SwimmingPoolSavedEventArgs e)
        {
            MessageBox.Show($"Swimming Pool '{e.SavedSwimmingPool.Name}' ({e.SavedSwimmingPool.PoolLength}m, {e.SavedSwimmingPool.NumberOfLanes.GetDisplayName()} lanes) has been successfully saved to the JSON file!", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateFunctionView_FunctionSaveRequested(object? sender, FunctionSavedEventArgs e)
        {
            MessageBox.Show($"Function '{e.SavedFunction.Name}' (Abbreviation: '{e.SavedFunction.Abbreviation}') has been successfully saved to the JSON file!", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateFunctionAssignmentView_FunctionAssignmentSaveRequested(object? sender, EventArgs e)
        {
            MessageBox.Show("Function Assignment saved successfully! (This will connect to data storage later)", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateUsersFunctionView_UsersFunctionSaveRequested(object? sender, EventArgs e)
        {
            MessageBox.Show("Users Function saved successfully! (This will connect to data storage later)", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateMeetFunctionView_MeetFunctionSaveRequested(object? sender, EventArgs e)
        {
            MessageBox.Show("Meet Function saved successfully! (This will connect to data storage later)", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
        }

        private void CreateJurysMemberView_JurysMemberSaveRequested(object? sender, EventArgs e)
        {
            MessageBox.Show("Jurys Member saved successfully! (This will connect to data storage later)", 
                          "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
            ShowDashboard();
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