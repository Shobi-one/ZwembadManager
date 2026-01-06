using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Events;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CreateClubViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _name = string.Empty;
        private string _abbreviation = string.Empty;
        private bool _isSaving;
        private string _saveButtonText = "💾 Save Club";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<ClubSavedEventArgs>? ClubSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Abbreviation
        {
            get => _abbreviation;
            set
            {
                if (_abbreviation != value)
                {
                    _abbreviation = value;
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

        public CreateClubViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(async () => await SaveClub(), () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task SaveClub()
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                IsSaving = true;
                SaveButtonText = "Saving...";

                // Create club with GUID automatically generated in constructor
                var club = new Club(Name.Trim(), Abbreviation.Trim());
                await _dataService.AddClubAsync(club);

                // Raise event with the saved club (including its GUID)
                ClubSaveRequested?.Invoke(this, new ClubSavedEventArgs(club));
                
                // Clear form after successful save
                ClearForm();
            }
            catch (InvalidOperationException ex)
            {
                // Handles duplicate club names/abbreviations
                MessageBox.Show(ex.Message,
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving club: {ex.Message}",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
                SaveButtonText = "💾 Save Club";
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Club Name is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(Abbreviation))
            {
                MessageBox.Show("Abbreviation is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (Abbreviation.Trim().Length > 10)
            {
                MessageBox.Show("Abbreviation should not exceed 10 characters.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            Name = string.Empty;
            Abbreviation = string.Empty;
            SaveButtonText = "💾 Save Club";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}