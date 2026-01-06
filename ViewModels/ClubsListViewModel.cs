using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using ZwembaadManager.Models;
using ZwembaadManager.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZwembaadManager.ViewModels
{
    public class ClubsListViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private ObservableCollection<Club> _clubs;
        private ObservableCollection<Club> _allClubs;
        private Club? _selectedClub;
        private Club? _filterClub;
        private bool _isLoading;

        public ObservableCollection<Club> Clubs
        {
            get => _clubs;
            set
            {
                if (_clubs != value)
                {
                    _clubs = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<Club> AllClubs
        {
            get => _allClubs;
            set
            {
                if (_allClubs != value)
                {
                    _allClubs = value;
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

        public Club? FilterClub
        {
            get => _filterClub;
            set
            {
                if (_filterClub != value)
                {
                    _filterClub = value;
                    OnPropertyChanged();
                    ApplyFilter();
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

        public ICommand SaveClubCommand { get; }
        public ICommand DeleteClubCommand { get; }
        public ICommand ClearFilterCommand { get; }

        public event EventHandler<string>? ClubSaved;
        public event PropertyChangedEventHandler? PropertyChanged;

        public ClubsListViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _clubs = new ObservableCollection<Club>();
            _allClubs = new ObservableCollection<Club>();

            SaveClubCommand = new RelayCommand(async () => await SaveClub(), () => _selectedClub != null);
            DeleteClubCommand = new RelayCommand(async () => await DeleteClub(), () => _selectedClub != null);
            ClearFilterCommand = new RelayCommand(() => FilterClub = null);
            _ = LoadClubs();
        }

        private async Task LoadClubs()
        {
            try
            {
                IsLoading = true;
                var clubs = await _dataService.LoadClubsAsync();
                AllClubs = new ObservableCollection<Club>(clubs);
                Clubs = new ObservableCollection<Club>(clubs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het laden van clubs: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFilter()
        {
            if (FilterClub == null)
            {
                Clubs = new ObservableCollection<Club>(AllClubs);
            }
            else
            {
                var filtered = AllClubs.Where(c => c.Id == FilterClub.Id).ToList();
                Clubs = new ObservableCollection<Club>(filtered);
            }
        }

        private async Task SaveClub()
        {
            if (SelectedClub == null) return;

            try
            {
                IsLoading = true;
                SelectedClub.ModifiedDate = DateTime.Now;
                var clubsList = new System.Collections.Generic.List<Club>(AllClubs);
                await _dataService.SaveClubsAsync(clubsList);
                MessageBox.Show("Club succesvol opgeslagen.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                ClubSaved?.Invoke(this, $"Club {SelectedClub.Name} opgeslagen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het opslaan van de club: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteClub()
        {
            if (SelectedClub == null) return;

            var result = MessageBox.Show($"Weet je zeker dat je '{SelectedClub.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                AllClubs.Remove(SelectedClub);
                Clubs.Remove(SelectedClub);
                var clubsList = new System.Collections.Generic.List<Club>(AllClubs);
                await _dataService.SaveClubsAsync(clubsList);
                MessageBox.Show("Club succesvol verwijderd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedClub = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het verwijderen van de club: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}