using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZwembaadManager.Classes;
using ZwembaadManager.Services;
using ZwembaadManager.Models;

namespace ZwembaadManager.ViewModels
{
    public class UsersListViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private readonly IDialogService _dialogService;
        private ObservableCollection<User> _users;
        private User? _selectedUser;
        private bool _isLoading;

        public ObservableCollection<User> Users
        {
            get => _users;
            set
            {
                if (_users != value)
                {
                    _users = value;
                    OnPropertyChanged();
                }
            }
        }

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged();
                    ((RelayCommand)SaveUserCommand).RaiseCanExecuteChanged();
                    ((RelayCommand)DeleteUserCommand).RaiseCanExecuteChanged();
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand SaveUserCommand { get; }
        public ICommand DeleteUserCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public UsersListViewModel(JsonDataService dataService, IDialogService dialogService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _dialogService = dialogService ?? throw new ArgumentNullException(nameof(dialogService));
            _users = new ObservableCollection<User>();

            SaveUserCommand = new RelayCommand(async () => await SaveUser(), () => _selectedUser != null);
            DeleteUserCommand = new RelayCommand(async () => await DeleteUser(), () => _selectedUser != null);
        }

        public async Task InitializeAsync()
        {
            await LoadUsers();
        }

        private async Task LoadUsers()
        {
            try
            {
                IsLoading = true;
                var users = await _dataService.LoadUsersAsync();
                Users = new ObservableCollection<User>(users);

                if (Users.Count > 0)
                {
                    SelectedUser = Users[0];
                }
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Fout bij het laden van gebruikers: {ex.Message}", "Fout");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveUser()
        {
            if (SelectedUser == null)
                return;

            try
            {
                IsLoading = true;
                var usersList = new List<User>(Users);
                await _dataService.SaveUsersAsync(usersList);
                _dialogService.ShowInformation("Gebruiker succesvol opgeslagen.", "Succes");
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Fout bij het opslaan van de gebruiker: {ex.Message}", "Fout");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteUser()
        {
            if (SelectedUser == null)
                return;

            var result = _dialogService.ShowConfirmation(
                $"Weet je zeker dat je '{SelectedUser.FirstName} {SelectedUser.LastName}' wilt verwijderen?",
                "Bevestiging");

            if (result != DialogResult.Yes)
                return;

            try
            {
                IsLoading = true;
                Users.Remove(SelectedUser);
                var usersList = new List<User>(Users);
                await _dataService.SaveUsersAsync(usersList);
                _dialogService.ShowInformation("Gebruiker succesvol verwijderd.", "Succes");
                SelectedUser = null;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError($"Fout bij het verwijderen van de gebruiker: {ex.Message}", "Fout");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}