using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace ZwembaadManager.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        public event EventHandler<string>? OpenViewRequested;
        public event EventHandler? LogoutRequested;

        public ICommand CreateUserCommand { get; }
        public ICommand CreateClubCommand { get; }
        public ICommand CreateMeetCommand { get; }
        public ICommand CreateSwimmingPoolCommand { get; }
        public ICommand CreateFunctionCommand { get; }
        public ICommand CreateUsersFunctionCommand { get; }
        public ICommand CreateMeetFunctionCommand { get; }
        public ICommand CreateJurysMemberCommand { get; }
        public ICommand ViewUsersCommand { get; }
        public ICommand ViewClubsCommand { get; }
        public ICommand ViewMeetsCommand { get; }
        public ICommand ViewSwimmingPoolsCommand { get; }
        public ICommand ViewFunctionsCommand { get; }
        public ICommand LogoutCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public DashboardViewModel()
        {
            // Quick Actions Commands
            CreateUserCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateUser"));
            CreateClubCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateClub"));
            CreateMeetCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateMeet"));
            CreateSwimmingPoolCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateSwimmingPool"));

            // Management Commands
            ViewUsersCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "UsersList"));
            ViewClubsCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "ClubsList"));
            ViewMeetsCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "MeetsList"));
            ViewSwimmingPoolsCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "SwimmingPoolsList"));
            ViewFunctionsCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "FunctionsList"));

            // Functions & Assignments Commands
            CreateFunctionCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateFunction"));
            CreateUsersFunctionCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateUsersFunction"));
            CreateMeetFunctionCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateMeetFunction"));

            // Jury Management Commands
            CreateJurysMemberCommand = new RelayCommand(() => OpenViewRequested?.Invoke(this, "CreateJurysMember"));

            // Logout Command
            LogoutCommand = new RelayCommand(() => LogoutRequested?.Invoke(this, EventArgs.Empty));
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}