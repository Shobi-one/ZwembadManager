using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CreateUsersFunctionViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _userId = string.Empty;
        private string _functionId = string.Empty;
        private string _status = "Active";
        private DateTime _startDate = DateTime.Today;
        private DateTime? _endDate;
        private string _remarks = string.Empty;
        private bool _isSaving;
        private string _saveButtonText = "💾 Save Assignment";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? UsersFunctionSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> Statuses { get; }

        public string UserId
        {
            get => _userId;
            set
            {
                if (_userId != value)
                {
                    _userId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string FunctionId
        {
            get => _functionId;
            set
            {
                if (_functionId != value)
                {
                    _functionId = value;
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

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(SaveUsersFunction, () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);
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
                MessageBox.Show($"User Function assignment saved:\nUser ID: {UserId}\nFunction ID: {FunctionId}\nStatus: {Status}\nStart Date: {StartDate:yyyy-MM-dd}",
                    "Save Placeholder",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                UsersFunctionSaveRequested?.Invoke(this, EventArgs.Empty);
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
            if (string.IsNullOrWhiteSpace(UserId))
            {
                MessageBox.Show("User ID is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(UserId, out _))
            {
                MessageBox.Show("User ID must be a valid number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(FunctionId))
            {
                MessageBox.Show("Function ID is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(FunctionId, out _))
            {
                MessageBox.Show("Function ID must be a valid number.", "Validation Error",
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
            UserId = string.Empty;
            FunctionId = string.Empty;
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