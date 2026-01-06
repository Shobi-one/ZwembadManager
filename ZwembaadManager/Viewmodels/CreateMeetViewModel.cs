using System;
using System.Collections.ObjectModel;
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
    public class CreateMeetViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private string _name = string.Empty;
        private DateTime _date = DateTime.Today;
        private string? _partOfTheDay;
        private string _meetState = "Planned";
        private string _clubId = string.Empty;
        private string _swimmingPoolId = string.Empty;
        private string _timeRegistration = string.Empty;
        private string _numberOfInternships = string.Empty;
        private string _numberOfExams = string.Empty;
        private string _remarks = string.Empty;
        private bool _isSaving;
        private string _saveButtonText = "💾 Save Meet";

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<MeetSavedEventArgs>? MeetSaveRequested;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<string> PartsOfDay { get; }
        public ObservableCollection<string> MeetStates { get; }

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

        public string SwimmingPoolId
        {
            get => _swimmingPoolId;
            set
            {
                if (_swimmingPoolId != value)
                {
                    _swimmingPoolId = value;
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

            // Initialize commands
            BackToDashboardCommand = new RelayCommand(() => BackToDashboardRequested?.Invoke(this, EventArgs.Empty));
            SaveCommand = new RelayCommand(async () => await SaveMeet(), () => !IsSaving);
            ClearCommand = new RelayCommand(ClearForm);
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

                // Set optional properties - NOW USING GUID
                if (!string.IsNullOrWhiteSpace(ClubId) && Guid.TryParse(ClubId, out Guid clubGuid))
                {
                    meet.ClubId = clubGuid;
                }

                if (!string.IsNullOrWhiteSpace(SwimmingPoolId) && Guid.TryParse(SwimmingPoolId, out Guid poolGuid))
                {
                    meet.SwimmingPoolId = poolGuid;
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

            if (!string.IsNullOrWhiteSpace(ClubId) && !Guid.TryParse(ClubId, out _))
            {
                MessageBox.Show("Club ID must be a valid GUID.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            if (!string.IsNullOrWhiteSpace(SwimmingPoolId) && !Guid.TryParse(SwimmingPoolId, out _))
            {
                MessageBox.Show("Swimming Pool ID must be a valid GUID.", "Validation Error",
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
            ClubId = string.Empty;
            SwimmingPoolId = string.Empty;
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