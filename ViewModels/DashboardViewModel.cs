using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using ZwembaadManager.Models;
using System.Windows.Input;

namespace ZwembaadManager.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler<string>? OpenViewRequested;
        public event EventHandler? LogoutRequested;

        // Commands voor Quick Actions
        public ICommand CreateUserCommand { get; }
        public ICommand CreateClubCommand { get; }
        public ICommand CreateMeetCommand { get; }
        public ICommand CreateSwimmingPoolCommand { get; }

        // Commands voor Management
        public ICommand ViewUsersCommand { get; }
        public ICommand ViewClubsCommand { get; }
        public ICommand ViewMeetsCommand { get; }
        public ICommand ViewSwimmingPoolsCommand { get; }
        public ICommand ViewFunctionsCommand { get; }

        // Commands voor Functions & Assignments
        public ICommand CreateFunctionCommand { get; }
        public ICommand CreateUsersFunctionCommand { get; }
        public ICommand CreateMeetFunctionCommand { get; }

        // Command voor Jury Management
        public ICommand CreateJurysMemberCommand { get; }

        // Logout Command
        public ICommand LogoutCommand { get; }

        public DashboardViewModel()
        {
            // Quick Actions
            CreateUserCommand = new RelayCommand(() => OpenView("CreateUserView"));
            CreateClubCommand = new RelayCommand(() => OpenView("CreateClubView"));
            CreateMeetCommand = new RelayCommand(() => OpenView("CreateMeetView"));
            CreateSwimmingPoolCommand = new RelayCommand(() => OpenView("CreateSwimmingPoolView"));

            // Management
            ViewUsersCommand = new RelayCommand(() => OpenView("UsersListView"));
            ViewClubsCommand = new RelayCommand(() => OpenView("ClubsListView"));
            ViewMeetsCommand = new RelayCommand(() => OpenView("MeetsListView"));
            ViewSwimmingPoolsCommand = new RelayCommand(() => OpenView("SwimmingPoolsListView"));
            ViewFunctionsCommand = new RelayCommand(() => OpenView("FunctionsListView"));

            // Functions & Assignments
            CreateFunctionCommand = new RelayCommand(() => OpenView("CreateFunctionView"));
            CreateUsersFunctionCommand = new RelayCommand(() => OpenView("CreateUsersFunctionView"));
            CreateMeetFunctionCommand = new RelayCommand(() => OpenView("CreateMeetFunctionView"));

            // Jury Management
            CreateJurysMemberCommand = new RelayCommand(() => OpenView("CreateJurysMemberView"));

            // Logout
            LogoutCommand = new RelayCommand(Logout);
        }

        private void OpenView(string viewName)
        {
            OpenViewRequested?.Invoke(this, viewName);
        }

        private void Logout()
        {
            LogoutRequested?.Invoke(this, EventArgs.Empty);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}