using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.ViewModels
{
    public class FunctionsListViewModel : INotifyPropertyChanged
    {
        private readonly JsonDataService _dataService;
        private ObservableCollection<Function> _functions;
        private Function? _selectedFunction;
        private bool _isLoading;

        public ObservableCollection<Function> Functions
        {
            get => _functions;
            set
            {
                if (_functions != value)
                {
                    _functions = value;
                    OnPropertyChanged();
                }
            }
        }

        public Function? SelectedFunction
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

        public ICommand LoadFunctionsCommand { get; }
        public ICommand SaveFunctionCommand { get; }
        public ICommand DeleteFunctionCommand { get; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public FunctionsListViewModel(JsonDataService dataService)
        {
            _dataService = dataService ?? throw new ArgumentNullException(nameof(dataService));
            _functions = new ObservableCollection<Function>();

            LoadFunctionsCommand = new RelayCommand(async () => await LoadFunctions());
            SaveFunctionCommand = new RelayCommand(async () => await SaveFunction(), () => _selectedFunction != null);
            DeleteFunctionCommand = new RelayCommand(async () => await DeleteFunction(), () => _selectedFunction != null);
        }

        private async Task LoadFunctions()
        {
            try
            {
                IsLoading = true;
                var functions = await _dataService.LoadFunctionsAsync();
                Functions = new ObservableCollection<Function>(functions);

                if (Functions.Count > 0)
                {
                    SelectedFunction = Functions[0];
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het laden van functies: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task SaveFunction()
        {
            if (SelectedFunction == null)
                return;

            try
            {
                IsLoading = true;
                SelectedFunction.ModifiedDate = DateTime.Now;
                var functionsList = new List<Function>(Functions);
                await _dataService.SaveFunctionsAsync(functionsList);
                MessageBox.Show("Functie succesvol opgeslagen.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het opslaan van de functie: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteFunction()
        {
            if (SelectedFunction == null)
                return;

            var result = MessageBox.Show($"Weet je zeker dat je '{SelectedFunction.Name}' wilt verwijderen?", "Bevestiging", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            try
            {
                IsLoading = true;
                Functions.Remove(SelectedFunction);
                var functionsList = new List<Function>(Functions);
                await _dataService.SaveFunctionsAsync(functionsList);
                MessageBox.Show("Functie succesvol verwijderd.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                SelectedFunction = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fout bij het verwijderen van de functie: {ex.Message}", "Fout", MessageBoxButton.OK, MessageBoxImage.Error);
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