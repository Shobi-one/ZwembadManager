using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Events;
using ZwembaadManager.Extensions;
using ZwembaadManager.Views;

namespace ZwembaadManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object? _currentView;

        public object? CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            // Start with login view
            NavigateToLogin();
        }

        private void NavigateToLogin()
        {
            var loginView = new LoginView();
            loginView.LoginSuccessful += (sender, e) => NavigateToDashboard();
            CurrentView = loginView;
        }

        private void NavigateToDashboard()
        {
            var dashboardView = new DashboardView();
            dashboardView.OpenViewRequested += (sender, viewType) => NavigateToView(viewType);
            dashboardView.LogoutRequested += (sender, e) => NavigateToLogin();
            CurrentView = dashboardView;
        }

        private void NavigateToView(string viewType)
        {
            switch (viewType)
            {
                case "CreateUser":
                    NavigateToCreateUser();
                    break;
                case "CreateClub":
                    NavigateToCreateClub();
                    break;
                case "ClubsList":
                    NavigateToClubsList();
                    break;
                case "CreateMeet":
                    NavigateToCreateMeet();
                    break;
                case "CreateSwimmingPool":
                    NavigateToCreateSwimmingPool();
                    break;
                case "CreateFunction":
                    NavigateToCreateFunction();
                    break;
                case "CreateUsersFunction":
                    NavigateToCreateUsersFunction();
                    break;
                case "CreateMeetFunction":
                    NavigateToCreateMeetFunction();
                    break;
                case "CreateJurysMember":
                    NavigateToCreateJurysMember();
                    break;
                case "CreateAddress":
                    MessageBox.Show("Create Address view will be implemented soon.");
                    break;
                case "MeetsList":
                    NavigateToMeetsList();
                    break;
                case "SwimmingPoolsList":
                    NavigateToSwimmingPoolsList();
                    break;
                case "FunctionsList":
                    NavigateToFunctionsList();
                    break;
                case "UsersList":
                    NavigateToUsersList();
                    break;
                default:
                    MessageBox.Show($"Unknown view type: {viewType}");
                    break;
            }
        }

        private void NavigateToCreateUser()
        {
            var view = new CreateUserView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.UserSaveRequested += (sender, e) =>
            {
                MessageBox.Show($"User '{e.SavedUser.FirstName} {e.SavedUser.LastName}' has been successfully saved to the JSON file!",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToCreateClub()
        {
            var view = new CreateClubView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.ClubSaveRequested += (sender, e) =>
            {
                MessageBox.Show($"Club '{e.SavedClub.Name}' (Abbreviation: '{e.SavedClub.Abbreviation}') has been successfully saved to the JSON file!",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToClubsList()
        {
            var view = new ClubsListView();
            view.BackRequested += (sender, e) => NavigateToDashboard();
            CurrentView = view;
        }

        private void NavigateToCreateMeet()
        {
            var view = new CreateMeetView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.MeetSaveRequested += (sender, e) =>
            {
                MessageBox.Show($"Meet '{e.SavedMeet.Name}' on {e.SavedMeet.Date:yyyy-MM-dd} ({e.SavedMeet.PartOfTheDay}) has been successfully saved to the JSON file!",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToCreateSwimmingPool()
        {
            var view = new CreateSwimmingPoolView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.SwimmingPoolSaveRequested += (sender, e) =>
            {
                MessageBox.Show($"Swimming Pool '{e.SavedSwimmingPool.Name}' ({e.SavedSwimmingPool.PoolLength}m, {e.SavedSwimmingPool.NumberOfLanes.GetDisplayName()} lanes) has been successfully saved to the JSON file!",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToCreateFunction()
        {
            var view = new CreateFunctionView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.FunctionSaveRequested += (sender, e) =>
            {
                MessageBox.Show($"Function '{e.SavedFunction.Name}' (Abbreviation: '{e.SavedFunction.Abbreviation}') has been successfully saved to the JSON file!",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToCreateUsersFunction()
        {
            var view = new CreateUsersFunctionView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.UsersFunctionSaveRequested += (sender, e) =>
            {
                MessageBox.Show("Users Function saved successfully! (This will connect to data storage later)",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToCreateMeetFunction()
        {
            var view = new CreateMeetFunctionView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.MeetFunctionSaveRequested += (sender, e) =>
            {
                MessageBox.Show("Meet Function saved successfully! (This will connect to data storage later)",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToCreateJurysMember()
        {
            var view = new CreateJurysMemberView();
            view.BackToDashboardRequested += (sender, e) => NavigateToDashboard();
            view.JurysMemberSaveRequested += (sender, e) =>
            {
                MessageBox.Show("Jury Member saved successfully! (This will connect to data storage later)",
                    "Save Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigateToDashboard();
            };
            CurrentView = view;
        }

        private void NavigateToMeetsList()
        {
            var view = new MeetsListView();
            view.BackRequested += (sender, e) => NavigateToDashboard();
            CurrentView = view;
        }

        private void NavigateToSwimmingPoolsList()
        {
            var view = new SwimmingPoolsListView();
            view.BackRequested += (sender, e) => NavigateToDashboard();
            CurrentView = view;
        }

        private void NavigateToFunctionsList()
        {
            var view = new FunctionsListView();
            view.BackRequested += (sender, e) => NavigateToDashboard();
            CurrentView = view;
        }

        private void NavigateToUsersList()
        {
            var view = new UsersListView();
            view.BackRequested += (sender, e) => NavigateToDashboard();
            CurrentView = view;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}