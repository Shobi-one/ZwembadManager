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
    public class CreateJurysMemberViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private User? _selectedOfficial;
        private Meet? _selectedMeet;
        private string _selectedFunction = string.Empty;
        private DateTime _assignmentDate = DateTime.Today;
        private string _notes = string.Empty;
        private bool _isSaving;
        private bool _isLoadingOfficials;
        private bool _isLoadingMeets;
        private bool _isLoadingFunctions;
        private string _saveButtonText = "💾 Save Assignment";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? JurysMemberSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<User> AvailableOfficials { get; }
        public ObservableCollection<Meet> AvailableMeets { get; }
        public ObservableCollection<Function> Functions { get; }

        public User? SelectedOfficial
        {
            get => _selectedOfficial;
            set
            {
                if (_selectedOfficial != value)
                {
                    _selectedOfficial = value;
                    OnPropertyChanged();
                }
            }
        }

        public Meet? SelectedMeet
        {
            get => _selectedMeet;
            set
            {
                if (_selectedMeet != value)
                {
                    _selectedMeet = value;
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

        public bool IsLoadingOfficials
        {
            get => _isLoadingOfficials;
            set
            {
                if (_isLoadingOfficials != value)
                {
                    _isLoadingOfficials = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoadingMeets
        {
            get => _isLoadingMeets;
            set
            {
                if (_isLoadingMeets != value)
                {
                    _isLoadingMeets = value;
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

        public CreateJurysMemberViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize collections
            AvailableOfficials = new ObservableCollection<User>();
            AvailableMeets = new ObservableCollection<Meet>();
            Functions = new ObservableCollection<Function>();

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(SaveJurysMember, () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);

            // Load data asynchronously
            _ = LoadOfficialsAsync();
            _ = LoadMeetsAsync();
            _ = LoadFunctionsAsync();
        }

        private async Task LoadOfficialsAsync()
        {
            try
            {
                IsLoadingOfficials = true;
                var users = await _dataService.LoadUsersAsync();
                
                AvailableOfficials.Clear();
                foreach (var user in users.OrderBy(u => u.LastName).ThenBy(u => u.FirstName))
                {
                    AvailableOfficials.Add(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading officials: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoadingOfficials = false;
            }
        }

        private async Task LoadMeetsAsync()
        {
            try
            {
                IsLoadingMeets = true;
                var meets = await _dataService.LoadMeetsAsync();
                
                AvailableMeets.Clear();
                foreach (var meet in meets.OrderByDescending(m => m.Date))
                {
                    AvailableMeets.Add(meet);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading meets: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoadingMeets = false;
            }
        }

        private async Task LoadFunctionsAsync()
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
                var selectedFunc = Functions.FirstOrDefault(f => f.Name == SelectedFunction);
                var functionInfo = selectedFunc != null ? $" ({selectedFunc.Name} - {selectedFunc.Abbreviation})" : "";
                
                MessageBox.Show($"Jury member assignment saved:\n" +
                              $"Official: {SelectedOfficial!.FirstName} {SelectedOfficial.LastName}\n" +
                              $"Meet: {SelectedMeet!.Name} on {SelectedMeet.Date:yyyy-MM-dd}\n" +
                              $"Function: {SelectedFunction}{functionInfo}\n" +
                              $"Assignment Date: {AssignmentDate:yyyy-MM-dd}\n" +
                              $"Notes: {Notes}",
                    "Assignment Saved",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                JurysMemberSaveRequested?.Invoke(this, EventArgs.Empty);
                ClearForm();
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
            if (SelectedOfficial == null)
            {
                MessageBox.Show("Please select an official.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (SelectedMeet == null)
            {
                MessageBox.Show("Please select a meet.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(SelectedFunction))
            {
                MessageBox.Show("Please select a function.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            SelectedOfficial = null;
            SelectedMeet = null;
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