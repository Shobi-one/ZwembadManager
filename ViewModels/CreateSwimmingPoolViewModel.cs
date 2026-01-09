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
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CreateSwimmingPoolViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _name = string.Empty;
        private string _poolLength = string.Empty;
        private NumberOfLanes? _numberOfLanes;
        private Address? _selectedAddress;
        private bool _isSaving;
        private bool _isLoadingAddresses;
        private string _saveButtonText = "💾 Save Pool";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<SwimmingPoolSavedEventArgs>? SwimmingPoolSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> PoolLengths { get; }
        public ObservableCollection<NumberOfLanesOption> NumberOfLanesOptions { get; }
        public ObservableCollection<Address> AvailableAddresses { get; }

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

        public string PoolLength
        {
            get => _poolLength;
            set
            {
                if (_poolLength != value)
                {
                    _poolLength = value;
                    OnPropertyChanged();
                }
            }
        }

        public NumberOfLanes? NumberOfLanes
        {
            get => _numberOfLanes;
            set
            {
                if (_numberOfLanes != value)
                {
                    _numberOfLanes = value;
                    OnPropertyChanged();
                }
            }
        }

        public Address? SelectedAddress
        {
            get => _selectedAddress;
            set
            {
                if (_selectedAddress != value)
                {
                    _selectedAddress = value;
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

        public bool IsLoadingAddresses
        {
            get => _isLoadingAddresses;
            set
            {
                if (_isLoadingAddresses != value)
                {
                    _isLoadingAddresses = value;
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

        public CreateSwimmingPoolViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize collections
            PoolLengths = new ObservableCollection<string>
            {
                "25",
                "50",
                "33.3",
                "20"
            };

            NumberOfLanesOptions = new ObservableCollection<NumberOfLanesOption>(
                Enum.GetValues<NumberOfLanes>()
                    .Select(lanes => new NumberOfLanesOption
                    {
                        DisplayName = lanes.GetDisplayName(),
                        Value = lanes
                    })
            );

            AvailableAddresses = new ObservableCollection<Address>();

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(async () => await SaveSwimmingPool(), () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);

            // Load addresses from JSON
            _ = LoadAddressesAsync();
        }

        private async Task LoadAddressesAsync()
        {
            try
            {
                IsLoadingAddresses = true;
                var addresses = await _dataService.LoadAddressesAsync();
                
                AvailableAddresses.Clear();
                foreach (var address in addresses.OrderBy(a => a.Street))
                {
                    AvailableAddresses.Add(address);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading addresses: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoadingAddresses = false;
            }
        }

        private async Task SaveSwimmingPool()
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                IsSaving = true;
                SaveButtonText = "Saving...";

                // Parse pool length
                decimal poolLength = decimal.Parse(PoolLength);

                // Create new swimming pool object
                var swimmingPool = new SwimmingPool(
                    Name.Trim(),
                    poolLength,
                    NumberOfLanes!.Value
                );

                // Set optional AddressId from selected address
                if (SelectedAddress != null)
                {
                    swimmingPool.AddressId = SelectedAddress.Id;
                }

                // Save to JSON file using JsonDataService
                await _dataService.AddSwimmingPoolAsync(swimmingPool);

                // Raise the event with the saved swimming pool
                SwimmingPoolSaveRequested?.Invoke(this, new SwimmingPoolSavedEventArgs(swimmingPool));
                
                // Clear form after successful save
                ClearForm();
            }
            catch (InvalidOperationException ex)
            {
                // Handles duplicate swimming pools
                MessageBox.Show(ex.Message,
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving swimming pool: {ex.Message}",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
                SaveButtonText = "💾 Save Pool";
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Pool Name is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(PoolLength))
            {
                MessageBox.Show("Pool Length is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!decimal.TryParse(PoolLength, out decimal lengthValue) || lengthValue <= 0)
            {
                MessageBox.Show("Pool Length must be a valid positive number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!NumberOfLanes.HasValue)
            {
                MessageBox.Show("Number of Lanes is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            Name = string.Empty;
            PoolLength = string.Empty;
            NumberOfLanes = null;
            SelectedAddress = null;
            SaveButtonText = "💾 Save Pool";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Helper class for ComboBox binding
    public class NumberOfLanesOption
    {
        public string DisplayName { get; set; } = string.Empty;
        public NumberOfLanes Value { get; set; }
    }
}