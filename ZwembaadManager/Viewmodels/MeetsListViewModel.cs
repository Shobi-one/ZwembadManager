using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using ZwembaadManager.Models;
using ZwembaadManager.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZwembaadManager.ViewModels
{
    public class MeetsListViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private ObservableCollection<Meet> _meets;
        private Meet? _selectedMeet;
        private bool _isLoading;

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

        public ICommand LoadMeetsCommand { get; }
        public ICommand SaveMeetCommand { get; }
        public ICommand DeleteMeetCommand { get; }

        public event EventHandler<string>? MeetSaved;
        public event PropertyChangedEventHandler? PropertyChanged;

        public MeetsListViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _meets = new ObservableCollection<Meet>();

            LoadMeetsCommand = new RelayCommand(async () => await LoadMeets());
            SaveMeetCommand = new RelayCommand(async () => await SaveMeet(), () => _selectedMeet != null);
            DeleteMeetCommand = new RelayCommand(async () => await DeleteMeet(), () => _selectedMeet != null);
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

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}