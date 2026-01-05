using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class CalendarDay : INotifyPropertyChanged
    {
        private int _day;
        private ObservableCollection<Meet> _meets;
        private Brush _background;
        private Brush _foreground;

        public int Day
        {
            get => _day;
            set
            {
                if (_day != value)
                {
                    _day = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Meet> Meets
        {
            get => _meets;
            set
            {
                if (_meets != value)
                {
                    _meets = value;
                    OnPropertyChanged();
                }
            }
        }

        public Brush Background
        {
            get => _background;
            set
            {
                if (_background != value)
                {
                    _background = value;
                    OnPropertyChanged();
                }
            }
        }

        public Brush Foreground
        {
            get => _foreground;
            set
            {
                if (_foreground != value)
                {
                    _foreground = value;
                    OnPropertyChanged();
                }
            }
        }

        public CalendarDay()
        {
            _meets = new ObservableCollection<Meet>();
            _background = Brushes.White;
            _foreground = Brushes.Black;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class MeetsListViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private ObservableCollection<Meet> _meets;
        private Meet? _selectedMeet;
        private bool _isLoading;
        private ObservableCollection<CalendarDay> _calendarDays;
        private DateTime _currentMonth;
        private string _currentMonthYear;

        public ObservableCollection<Meet> Meets
        {
            get => _meets;
            set
            {
                if (_meets != value)
                {
                    _meets = value;
                    OnPropertyChanged();
                    GenerateCalendar();
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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<CalendarDay> CalendarDays
        {
            get => _calendarDays;
            set
            {
                if (_calendarDays != value)
                {
                    _calendarDays = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CurrentMonthYear
        {
            get => _currentMonthYear;
            set
            {
                if (_currentMonthYear != value)
                {
                    _currentMonthYear = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadMeetsCommand { get; }
        public ICommand SaveMeetCommand { get; }
        public ICommand DeleteMeetCommand { get; }
        public ICommand PreviousMonthCommand { get; }
        public ICommand NextMonthCommand { get; }

        public event EventHandler<string>? MeetSaved;
        public event PropertyChangedEventHandler? PropertyChanged;

        public MeetsListViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _meets = new ObservableCollection<Meet>();
            _calendarDays = new ObservableCollection<CalendarDay>();
            _currentMonth = DateTime.Today;
            _currentMonthYear = string.Empty;

            LoadMeetsCommand = new RelayCommand(async () => await LoadMeets());
            SaveMeetCommand = new RelayCommand(async () => await SaveMeet(), () => _selectedMeet != null);
            DeleteMeetCommand = new RelayCommand(async () => await DeleteMeet(), () => _selectedMeet != null);
            PreviousMonthCommand = new RelayCommand(() => NavigateMonth(-1));
            NextMonthCommand = new RelayCommand(() => NavigateMonth(1));

            GenerateCalendar();
        }

        private async Task LoadMeets()
        {
            try
            {
                IsLoading = true;
                var meets = await _dataService.LoadMeetsAsync();
                Meets = new ObservableCollection<Meet>(meets);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het laden van meetings: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveMeet()
        {
            if (SelectedMeet == null) return;

            try
            {
                IsLoading = true;
                SelectedMeet.ModifiedDate = DateTime.Now;
                var meetsList = new System.Collections.Generic.List<Meet>(Meets);
                await _dataService.SaveMeetsAsync(meetsList);
                MessageBox.Show("Meeting succesvol opgeslagen.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                MeetSaved?.Invoke(this, $"Meet {SelectedMeet.Name} opgeslagen");
                GenerateCalendar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het opslaan van de meeting: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteMeet()
        {
            if (SelectedMeet == null) return;

            var result = MessageBox.Show($"Weet je zeker dat je '{SelectedMeet.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                Meets.Remove(SelectedMeet);
                var meetsList = new System.Collections.Generic.List<Meet>(Meets);
                await _dataService.SaveMeetsAsync(meetsList);
                MessageBox.Show("Meeting succesvol verwijderd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedMeet = null;
                GenerateCalendar();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het verwijderen van de meeting: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void NavigateMonth(int direction)
        {
            _currentMonth = _currentMonth.AddMonths(direction);
            GenerateCalendar();
        }

        private void GenerateCalendar()
        {
            var calendar = new ObservableCollection<CalendarDay>();
            var firstDayOfMonth = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            var culture = new CultureInfo("nl-NL");
            CurrentMonthYear = firstDayOfMonth.ToString("MMMM yyyy", culture);

            // Get the day of week (Monday = 0, Sunday = 6)
            var startDayOfWeek = (int)firstDayOfMonth.DayOfWeek;
            startDayOfWeek = startDayOfWeek == 0 ? 6 : startDayOfWeek - 1; // Adjust so Monday = 0

            var previousMonth = firstDayOfMonth.AddMonths(-1);
            var daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
            for (var i = startDayOfWeek - 1; i >= 0; i--)
            {
                calendar.Add(new CalendarDay
                {
                    Day = daysInPreviousMonth - i,
                    Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    Foreground = Brushes.Gray,
                    Meets = new ObservableCollection<Meet>()
                });
            }

            var today = DateTime.Today;
            for (var day = 1; day <= lastDayOfMonth.Day; day++)
            {
                var currentDate = new DateTime(_currentMonth.Year, _currentMonth.Month, day);
                var meetsOnDay = Meets?.Where(m => m.Date.Date == currentDate.Date).ToList() ?? new System.Collections.Generic.List<Meet>();

                var calendarDay = new CalendarDay
                {
                    Day = day,
                    Meets = new ObservableCollection<Meet>(meetsOnDay)
                };

                if (currentDate.Date == today)
                {
                    calendarDay.Background = new SolidColorBrush(Color.FromRgb(255, 248, 220));
                    calendarDay.Foreground = Brushes.Black;
                }
                else
                {
                    calendarDay.Background = Brushes.White;
                    calendarDay.Foreground = Brushes.Black;
                }

                calendar.Add(calendarDay);
            }

            // Add days from next month to fill the grid (6 rows × 7 columns = 42 cells)
            var remainingCells = 42 - calendar.Count;
            for (var day = 1; day <= remainingCells; day++)
            {
                calendar.Add(new CalendarDay
                {
                    Day = day,
                    Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                    Foreground = Brushes.Gray,
                    Meets = new ObservableCollection<Meet>()
                });
            }

            CalendarDays = calendar;
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}