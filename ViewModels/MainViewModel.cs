using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Events;

namespace ZwembaadManager.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private object? _currentView;
        private string? _currentUsername;

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

        public string? CurrentUsername
        {
            get => _currentUsername;
            set
            {
                if (_currentUsername != value)
                {
                    _currentUsername = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel()
        {
            // Start with Login View
            ShowLogin();
        }

        private void ShowLogin()
        {
            var loginViewType = Type.GetType("ZwembaadManager.Views.LoginView, ZwembaadManager.Views");
            if (loginViewType != null)
            {
                var loginView = Activator.CreateInstance(loginViewType);
                
                // Subscribe to LoginSuccessful event using reflection
                var loginSuccessfulEvent = loginViewType.GetEvent("LoginSuccessful");
                if (loginSuccessfulEvent != null)
                {
                    var handler = new EventHandler<LoginEventArgs>(OnLoginSuccessful);
                    loginSuccessfulEvent.AddEventHandler(loginView, handler);
                }
                
                CurrentView = loginView;
            }
        }

        private void OnLoginSuccessful(object? sender, LoginEventArgs e)
        {
            CurrentUsername = e.Username;
            ShowDashboard();
        }

            private void ShowDashboard()
        {
            var dashboardViewType = Type.GetType("ZwembaadManager.Views.DashboardView, ZwembaadManager.Views");
            if (dashboardViewType != null)
            {
                var dashboardView = Activator.CreateInstance(dashboardViewType);
                
                // Subscribe to navigation events
                var openViewEvent = dashboardViewType.GetEvent("OpenViewRequested");
                if (openViewEvent != null)
                {
                    var handler = new EventHandler<string>(OnOpenViewRequested);
                    openViewEvent.AddEventHandler(dashboardView, handler);
                }
                
                var logoutEvent = dashboardViewType.GetEvent("LogoutRequested");
                if (logoutEvent != null)
                {
                    var handler = new EventHandler(OnLogoutRequested);
                    logoutEvent.AddEventHandler(dashboardView, handler);
                }
                
                CurrentView = dashboardView;
            }
        }

        private void OnOpenViewRequested(object? sender, string viewName)
        {
            NavigateToView(viewName);
        }

        private void OnLogoutRequested(object? sender, EventArgs e)
        {
            CurrentUsername = null;
            ShowLogin();
        }

        private void NavigateToView(string viewName)
        {
            var viewType = Type.GetType($"ZwembaadManager.Views.{viewName}, ZwembaadManager.Views");
            if (viewType != null)
            {
                var view = Activator.CreateInstance(viewType);
                
                // Subscribe to BackRequested or BackToDashboardRequested events
                var backRequestedEvent = viewType.GetEvent("BackRequested");
                if (backRequestedEvent != null)
                {
                    var handler = new EventHandler(OnBackToDashboardRequested);
                    backRequestedEvent.AddEventHandler(view, handler);
                }
                
                var backToDashboardEvent = viewType.GetEvent("BackToDashboardRequested");
                if (backToDashboardEvent != null)
                {
                    var handler = new EventHandler(OnBackToDashboardRequested);
                    backToDashboardEvent.AddEventHandler(view, handler);
                }
                
                CurrentView = view;
            }
            else
            {
                MessageBox.Show($"View '{viewName}' kon niet worden geladen.", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OnBackToDashboardRequested(object? sender, EventArgs e)
        {
            ShowDashboard();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}