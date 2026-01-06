using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using ZwembaadManager.Models;
using ZwembaadManager.Services;
using ZwembaadManager.Classes.Enum;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ZwembaadManager.ViewModels
{
    public class SwimmingPoolsListViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private ObservableCollection<SwimmingPool> _swimmingPools;
        private SwimmingPool? _selectedSwimmingPool;
        private bool _isLoading;

        public ObservableCollection<SwimmingPool> SwimmingPools
        {
            get => _swimmingPools;
            set
            {
                if (_swimmingPools != value)
                {
                    _swimmingPools = value;
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

        public ICommand SaveSwimmingPoolCommand { get; }
        public ICommand DeleteSwimmingPoolCommand { get; }

        public event EventHandler<string>? SwimmingPoolSaved;
        public event PropertyChangedEventHandler? PropertyChanged;

        public SwimmingPoolsListViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _swimmingPools = new ObservableCollection<SwimmingPool>();

            SaveSwimmingPoolCommand = new RelayCommand(async () => await SaveSwimmingPool(), () => _selectedSwimmingPool != null);
            DeleteSwimmingPoolCommand = new RelayCommand(async () => await DeleteSwimmingPool(), () => _selectedSwimmingPool != null);
            _ = LoadSwimmingPools();
        }

        private async Task LoadSwimmingPools()
        {
            try
            {
                IsLoading = true;
                var swimmingPools = await _dataService.LoadSwimmingPoolsAsync();
                SwimmingPools = new ObservableCollection<SwimmingPool>(swimmingPools);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het laden van zwembaden: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveSwimmingPool()
        {
            if (SelectedSwimmingPool == null) return;

            try
            {
                IsLoading = true;
                SelectedSwimmingPool.ModifiedDate = DateTime.Now;
                var swimmingPoolsList = new System.Collections.Generic.List<SwimmingPool>(SwimmingPools);
                await _dataService.SaveSwimmingPoolsAsync(swimmingPoolsList);
                MessageBox.Show("Zwembad succesvol opgeslagen.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                SwimmingPoolSaved?.Invoke(this, $"Zwembad {SelectedSwimmingPool.Name} opgeslagen");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het opslaan van het zwembad: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteSwimmingPool()
        {
            if (SelectedSwimmingPool == null) return;

            var result = MessageBox.Show($"Weet je zeker dat je '{SelectedSwimmingPool.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                IsLoading = true;
                SwimmingPools.Remove(SelectedSwimmingPool);
                var swimmingPoolsList = new System.Collections.Generic.List<SwimmingPool>(SwimmingPools);
                await _dataService.SaveSwimmingPoolsAsync(swimmingPoolsList);
                MessageBox.Show("Zwembad succesvol verwijderd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedSwimmingPool = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het verwijderen van het zwembad: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
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