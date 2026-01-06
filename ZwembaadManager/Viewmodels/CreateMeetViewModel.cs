using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using ZwembaadManager.Events;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CreateMeetViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _name = string.Empty;
        private DateTime _date = DateTime.Today;
        private string? _partOfTheDay;
        private string _meetState = "Planned";
        private Club? _selectedClub;
        private SwimmingPool? _selectedSwimmingPool;
        private string _timeRegistration = string.Empty;
        private string _numberOfInternships = string.Empty;
        private string _numberOfExams = string.Empty;
        private string _remarks = string.Empty;
        private bool _isSaving;
        private bool _isLoadingClubs;
        private bool _isLoadingPools;
        private string _saveButtonText = "💾 Save Meet";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<MeetSavedEventArgs>? MeetSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> PartsOfDay { get; }
        public ObservableCollection<string> MeetStates { get; }
        public ObservableCollection<Club> AvailableClubs { get; }
        public ObservableCollection<SwimmingPool> AvailableSwimmingPools { get; }

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

        public DateTime Date
        {
            get => _date;
            set
            {
                if (_date != value)
                {
                    _date = value;
                    OnPropertyChanged();
                }
            }
        }

        public string? PartOfTheDay
        {
            get => _partOfTheDay;
            set
            {
                if (_partOfTheDay != value)
                {
                    _partOfTheDay = value;
                    OnPropertyChanged();
                }
            }
        }

        public string MeetState
        {
            get => _meetState;
            set
            {
                if (_meetState != value)
                {
                    _meetState = value;
                    OnPropertyChanged();
                }
            }
        }

        public Club? SelectedClub
        {
            get => _selectedClub;
            set
            {
                if (_selectedClub != value)
                {
                    _selectedClub = value;
                    OnPropertyChanged();
                }
            }
        }

        public SwimmingPool? SelectedSwimmingPool
        {
            get => _selectedSwimmingPool;
            set
            {
                if (_selectedSwimmingPool != value)
                {
                    _selectedSwimmingPool = value;
                    OnPropertyChanged();
                }
            }
        }

        public string TimeRegistration
        {
            get => _timeRegistration;
            set
            {
                if (_timeRegistration != value)
                {
                    _timeRegistration = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NumberOfInternships
        {
            get => _numberOfInternships;
            set
            {
                if (_numberOfInternships != value)
                {
                    _numberOfInternships = value;
                    OnPropertyChanged();
                }
            }
        }

        public string NumberOfExams
        {
            get => _numberOfExams;
            set
            {
                if (_numberOfExams != value)
                {
                    _numberOfExams = value;
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

        public bool IsLoadingClubs
        {
            get => _isLoadingClubs;
            set
            {
                if (_isLoadingClubs != value)
                {
                    _isLoadingClubs = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoadingPools
        {
            get => _isLoadingPools;
            set
            {
                if (_isLoadingPools != value)
                {
                    _isLoadingPools = value;
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

        public CreateMeetViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));

            // Initialize collections
            PartsOfDay = new ObservableCollection<string>
            {
                "Morning",
                "Afternoon",
                "Evening",
                "All Day"
            };

            MeetStates = new ObservableCollection<string>
            {
                "Planned",
                "Active",
                "Completed",
                "Cancelled"
            };

            AvailableClubs = new ObservableCollection<Club>();
            AvailableSwimmingPools = new ObservableCollection<SwimmingPool>();

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(async () => await SaveMeet(), () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);

            // Load data asynchronously
            _ = LoadClubsAsync();
            _ = LoadSwimmingPoolsAsync();
        }

        private async Task LoadClubsAsync()
        {
            try
            {
                IsLoadingClubs = true;
                var clubs = await _dataService.LoadClubsAsync();
                
                AvailableClubs.Clear();
                foreach (var club in clubs.OrderBy(c => c.Name))
                {
                    AvailableClubs.Add(club);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading clubs: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoadingClubs = false;
            }
        }

        private async Task LoadSwimmingPoolsAsync()
        {
            try
            {
                IsLoadingPools = true;
                var pools = await _dataService.LoadSwimmingPoolsAsync();
                
                AvailableSwimmingPools.Clear();
                foreach (var pool in pools.OrderBy(p => p.Name))
                {
                    AvailableSwimmingPools.Add(pool);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading swimming pools: {ex.Message}", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            finally
            {
                IsLoadingPools = false;
            }
        }

        private async Task SaveMeet()
        {
            if (!ValidateForm())
            {
                return;
            }

            try
            {
                IsSaving = true;
                SaveButtonText = "Saving...";

                // Create new meet object
                var meet = new Meet(
                    Name.Trim(),
                    Date,
                    PartOfTheDay!,
                    MeetState
                );

                // Set optional properties using selected objects
                if (SelectedClub != null)
                {
                    meet.ClubId = SelectedClub.Id;
                }

                if (SelectedSwimmingPool != null)
                {
                    meet.SwimmingPoolId = SelectedSwimmingPool.Id;
                }

                meet.TimeRegistration = TimeRegistration.Trim();

                if (!string.IsNullOrWhiteSpace(NumberOfInternships) && int.TryParse(NumberOfInternships, out int internships))
                {
                    meet.NumberOfInternships = internships;
                }

                if (!string.IsNullOrWhiteSpace(NumberOfExams) && int.TryParse(NumberOfExams, out int exams))
                {
                    meet.NumberOfExams = exams;
                }

                meet.Remarks = Remarks.Trim();

                // Save to JSON file using JsonDataService
                await _dataService.AddMeetAsync(meet);

                // Raise the event with the saved meet
                MeetSaveRequested?.Invoke(this, new MeetSavedEventArgs(meet));
                
                // Clear form after successful save
                ClearForm();
            }
            catch (InvalidOperationException ex)
            {
                // Handles duplicate meets
                MessageBox.Show(ex.Message,
                    "Validation Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving meet: {ex.Message}",
                    "Save Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
                SaveButtonText = "💾 Save Meet";
            }
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                MessageBox.Show("Meet Name is required.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(PartOfTheDay))
            {
                MessageBox.Show("Please select a part of the day.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(MeetState))
            {
                MessageBox.Show("Please select a meet state.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(NumberOfInternships) && !int.TryParse(NumberOfInternships, out _))
            {
                MessageBox.Show("Number of Internships must be a valid number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(NumberOfExams) && !int.TryParse(NumberOfExams, out _))
            {
                MessageBox.Show("Number of Exams must be a valid number.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            Name = string.Empty;
            Date = DateTime.Today;
            PartOfTheDay = null;
            MeetState = "Planned";
            SelectedClub = null;
            SelectedSwimmingPool = null;
            TimeRegistration = string.Empty;
            NumberOfInternships = string.Empty;
            NumberOfExams = string.Empty;
            Remarks = string.Empty;
            SaveButtonText = "💾 Save Meet";
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}