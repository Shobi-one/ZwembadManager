using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Classes;
using ZwembaadManager.Classes.Enum;
using ZwembaadManager.Events;
using ZwembaadManager.Extensions;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CreateUserViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _email = string.Empty;
        private string _mobile = string.Empty;
        private string _clubId = string.Empty;
        private string _license = string.Empty;
        private string _county = string.Empty;
        private object? _selectedGender;
        private bool _isSaving;
        private string _saveButtonText = "?? Save User";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<UserSavedEventArgs>? UserSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (_firstName != value)
                {
                    _firstName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (_lastName != value)
                {
                    _lastName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Mobile
        {
            get => _mobile;
            set
            {
                if (_mobile != value)
                {
                    _mobile = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ClubId
        {
            get => _clubId;
            set
            {
                if (_clubId != value)
                {
                    _clubId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string License
        {
            get => _license;
            set
            {
                if (_license != value)
                {
                    _license = value;
                    OnPropertyChanged();
                }
            }
        }

        public string County
        {
            get => _county;
            set
            {
                if (_county != value)
                {
                    _county = value;
                    OnPropertyChanged();
                }
            }
        }

        public object? SelectedGender
        {
            get => _selectedGender;
            set
            {
                if (_selectedGender != value)
                {
                    _selectedGender = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsSaving
        {
            get => _isSaving;
            set
            {
                if (_isSaving != value)
                {
                    _isSaving = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsNotSaving));
                }
            }
        }

        public bool IsNotSaving => !IsSaving;

        public string SaveButtonText
        {
            get => _saveButtonText;
            set
            {
                if (_saveButtonText != value)
                {
                    _saveButtonText = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<object> GenderOptions { get; }

        public ICommand BackToDashboardCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }

        public CreateUserViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Load gender options
            GenderOptions = new ObservableCollection<object>(
                Enum.GetValues<Gender>()
                    .Select(g => new { DisplayName = g.GetDisplayName(), Value = g })
                    .Cast<object>()
            );

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(async () => await SaveUser(), () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task SaveUser()
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                IsSaving = true;
                SaveButtonText = "Saving...";

                var newUser = CreateUserFromForm();
                await _dataService.AddUserAsync(newUser);

                UserSaveRequested?.Invoke(this, new UserSavedEventArgs(newUser));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user: {ex.Message}", "Save Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
                SaveButtonText = "?? Save User";
            }
        }

        private User CreateUserFromForm()
        {
            var user = new User
            {
                FirstName = FirstName.Trim(),
                LastName = LastName.Trim(),
                Email = Email.Trim(),
                Mobile = Mobile.Trim(),
                License = License.Trim(),
                County = County.Trim(),
                IsActive = true
            };

            if (!string.IsNullOrWhiteSpace(ClubId) && int.TryParse(ClubId, out int clubId))
            {
                user.ClubId = clubId;
            }

            if (SelectedGender != null)
            {
                var genderValue = SelectedGender.GetType().GetProperty("Value")?.GetValue(SelectedGender);
                if (genderValue is Gender gender)
                {
                    user.KeycloakGroups = gender.ToString();
                }
            }

            return user;
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("First Name is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Last Name is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                MessageBox.Show("Email is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!Email.Contains("@") || !Email.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (SelectedGender == null)
            {
                MessageBox.Show("Gender is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(ClubId) && !int.TryParse(ClubId, out _))
            {
                MessageBox.Show("Club ID must be a valid number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            FirstName = string.Empty;
            LastName = string.Empty;
            Email = string.Empty;
            Mobile = string.Empty;
            ClubId = string.Empty;
            License = string.Empty;
            County = string.Empty;
            SelectedGender = null;
            SaveButtonText = "?? Save User";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}