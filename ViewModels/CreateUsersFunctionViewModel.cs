using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Classes;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CreateUsersFunctionViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private User? _selectedUser;
        private Function? _selectedFunction;
        private string _status = "Active";
        private DateTime _startDate = DateTime.Today;
        private DateTime? _endDate;
        private string _remarks = string.Empty;
        private bool _isSaving;
        private bool _isLoadingUsers;
        private bool _isLoadingFunctions;
        private string _saveButtonText = "💾 Save Assignment";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? UsersFunctionSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> Statuses { get; }
        public ObservableCollection<User> AvailableUsers { get; }
        public ObservableCollection<Function> AvailableFunctions { get; }

        public User? SelectedUser
        {
            get => _selectedUser;
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    OnPropertyChanged();
                }
            }
        }

        public Function? SelectedFunction
        {
            get => _selectedFunction;
            set
            {
                if (_selectedFunction != value)
                {
                    _selectedFunction = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                if (_startDate != value)
                {
                    _startDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime? EndDate
        {
            get => _endDate;
            set
            {
                if (_endDate != value)
                {
                    _endDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Remarks
        {
            get => _remarks;
            set
            {
                if (_remarks != value)
                {
                    _remarks = value;
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

        public bool IsLoadingUsers
        {
            get => _isLoadingUsers;
            set
            {
                if (_isLoadingUsers != value)
                {
                    _isLoadingUsers = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoadingFunctions
        {
            get => _isLoadingFunctions;
            set
            {
                if (_isLoadingFunctions != value)
                {
                    _isLoadingFunctions = value;
                    OnPropertyChanged();
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

        public ICommand BackToDashboardCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }

        public CreateUsersFunctionViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize collections
            Statuses = new ObservableCollection<string>
            {
                "Active",
                "Inactive",
                "Pending",
                "Suspended"
            };

            AvailableUsers = new ObservableCollection<User>();
            AvailableFunctions = new ObservableCollection<Function>();

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(SaveUsersFunction, () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);

            // Load data asynchronously
            _ = LoadUsersAsync();
            _ = LoadFunctionsAsync();
        }

        private async Task LoadUsersAsync()
        {
            try
            {
                IsLoadingUsers = true;
                var users = await _dataService.LoadUsersAsync();

                AvailableUsers.Clear();
                foreach (var user in users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName))
                {
                    AvailableUsers.Add(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoadingUsers = false;
            }
        }

        private async Task LoadFunctionsAsync()
        {
            try
            {
                IsLoadingFunctions = true;
                var functions = await _dataService.LoadFunctionsAsync();

                AvailableFunctions.Clear();
                foreach (var function in functions.OrderBy(f => f.Name))
                {
                    AvailableFunctions.Add(function);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading functions: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoadingFunctions = false;
            }
        }

        private void SaveUsersFunction()
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                IsSaving = true;
                SaveButtonText = "Saving...";

                // TODO: Create UsersFunction model and save to data service when model is ready
                // For now, just show success message
                MessageBox.Show($"User Function assignment saved:\n" +
                              $"User: {SelectedUser!.FirstName} {SelectedUser.LastName}\n" +
                              $"Function: {SelectedFunction!.Name} ({SelectedFunction.Abbreviation})\n" +
                              $"Status: {Status}\n" +
                              $"Start Date: {StartDate:yyyy-MM-dd}\n" +
                              $"End Date: {(EndDate.HasValue ? EndDate.Value.ToString("yyyy-MM-dd") : "N/A")}",
                    "Save Placeholder",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                UsersFunctionSaveRequested?.Invoke(this, EventArgs.Empty);
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving user function assignment: {ex.Message}",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
                SaveButtonText = "💾 Save Assignment";
            }
        }

        private bool ValidateForm()
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Please select a user.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (SelectedFunction == null)
            {
                MessageBox.Show("Please select a function.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Status))
            {
                MessageBox.Show("Please select a status.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (EndDate.HasValue && EndDate.Value < StartDate)
            {
                MessageBox.Show("End Date cannot be before Start Date.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            SelectedUser = null;
            SelectedFunction = null;
            Status = "Active";
            StartDate = DateTime.Today;
            EndDate = null;
            Remarks = string.Empty;
            SaveButtonText = "💾 Save Assignment";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}