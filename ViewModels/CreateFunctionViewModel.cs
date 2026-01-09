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
    public class CreateFunctionViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _name = string.Empty;
        private string _abbreviation = string.Empty;
        private bool _isSaving;
        private string _saveButtonText = "💾 Save Function";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<FunctionSavedEventArgs>? FunctionSaveRequested;
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

        public CreateFunctionViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(async () => await SaveFunction(), () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);
        }

        private async Task SaveFunction()
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                IsSaving = true;
                SaveButtonText = "Saving...";

                var function = new Function(Name.Trim(), Abbreviation.Trim());
                await _dataService.AddFunctionAsync(function);

                MessageBox.Show($"Function saved to: {_dataService.GetFunctionsFilePath()}",
                    "Debug Info",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                FunctionSaveRequested?.Invoke(this, new FunctionSavedEventArgs(function));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving function: {ex.Message}",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
                SaveButtonText = "💾 Save Function";
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Function Name is required.", "Validation Error",
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
            SaveButtonText = "💾 Save Function";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}