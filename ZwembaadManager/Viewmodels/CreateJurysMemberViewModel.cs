using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CreateJurysMemberViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _officialId = string.Empty;
        private string _meetId = string.Empty;
        private string _selectedFunction = string.Empty;
        private DateTime _assignmentDate = DateTime.Today;
        private string _notes = string.Empty;
        private bool _isSaving;
        private bool _isLoadingFunctions;
        private string _saveButtonText = "💾 Save Assignment";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? JurysMemberSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string OfficialId
        {
            get => _officialId;
            set
            {
                if (_officialId != value)
                {
                    _officialId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MeetId
        {
            get => _meetId;
            set
            {
                if (_meetId != value)
                {
                    _meetId = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedFunction
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

        public DateTime AssignmentDate
        {
            get => _assignmentDate;
            set
            {
                if (_assignmentDate != value)
                {
                    _assignmentDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Notes
        {
            get => _notes;
            set
            {
                if (_notes != value)
                {
                    _notes = value;
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

        public ObservableCollection<Function> Functions { get; }

        public ICommand BackToDashboardCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand ClearCommand { get; }

        public CreateJurysMemberViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize collections
            Functions = new ObservableCollection<Function>();

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(SaveJurysMember, () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);

            // Load functions from JSON
            LoadFunctionsAsync();
        }

        private async void LoadFunctionsAsync()
        {
            try
            {
                IsLoadingFunctions = true;
                var functions = await _dataService.LoadFunctionsAsync();
                
                Functions.Clear();
                foreach (var function in functions.OrderBy(f => f.Name))
                {
                    Functions.Add(function);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading functions: {ex.Message}",
                    "Load Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsLoadingFunctions = false;
            }
        }

        private void SaveJurysMember()
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                IsSaving = true;
                SaveButtonText = "Saving...";

                // TODO: Implement actual save logic when JurysMember model and service are ready
                // For now, just show success message
                var selectedFunc = Functions.FirstOrDefault(f => f.Name == SelectedFunction);
                var functionInfo = selectedFunc != null ? $" ({selectedFunc.Name} - {selectedFunc.Abbreviation})" : "";
                
                MessageBox.Show($"Jury member assignment would be saved here:\n" +
                              $"Official ID: {OfficialId}\n" +
                              $"Meet ID: {MeetId}\n" +
                              $"Function: {SelectedFunction}{functionInfo}\n" +
                              $"Assignment Date: {AssignmentDate:yyyy-MM-dd}\n" +
                              $"Notes: {Notes}\n\n" +
                              $"This functionality will be implemented when the data service is ready.",
                    "Assignment Info",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                JurysMemberSaveRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving assignment: {ex.Message}",
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
            if (string.IsNullOrWhiteSpace(OfficialId))
            {
                MessageBox.Show("Official ID is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(OfficialId, out _))
            {
                MessageBox.Show("Official ID must be a valid number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(MeetId))
            {
                MessageBox.Show("Meet ID is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!int.TryParse(MeetId, out _))
            {
                MessageBox.Show("Meet ID must be a valid number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SelectedFunction))
            {
                MessageBox.Show("Function selection is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            OfficialId = string.Empty;
            MeetId = string.Empty;
            SelectedFunction = string.Empty;
            AssignmentDate = DateTime.Today;
            Notes = string.Empty;
            SaveButtonText = "💾 Save Assignment";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}