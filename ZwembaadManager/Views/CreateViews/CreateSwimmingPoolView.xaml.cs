using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Classes.Enum;
using ZwembaadManager.Extensions;
using ZwembaadManager.Events;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateSwimmingPoolView.xaml
    /// </summary>
    public partial class CreateSwimmingPoolView : UserControl
    {
        private readonly JsonDataService _dataService;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<SwimmingPoolSavedEventArgs>? SwimmingPoolSaveRequested;

        public CreateSwimmingPoolView()
        {
            InitializeComponent();
            _dataService = new JsonDataService();
            LoadNumberOfLanesOptions();
        }

        private void LoadNumberOfLanesOptions()
        {
            var numberOfLanesOptions = Enum.GetValues<NumberOfLanes>()
                .Select(lanes => new { DisplayName = lanes.GetDisplayName(), Value = lanes })
                .ToList();

            cmbNumberOfLanes.ItemsSource = numberOfLanesOptions;
        }

        private void BtnBackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackToDashboardRequested?.Invoke(this, EventArgs.Empty);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    // Disable the save button to prevent multiple clicks
                    btnSave.IsEnabled = false;
                    btnSave.Content = "Saving...";

                    // Get the pool length
                    string poolLengthText = GetComboBoxValue(cmbPoolLength);
                    decimal poolLength = decimal.Parse(poolLengthText);

                    // Get the number of lanes
                    var selectedLanes = (NumberOfLanes)cmbNumberOfLanes.SelectedValue;

                    // Create new swimming pool object
                    var swimmingPool = new SwimmingPool(
                        txtName.Text.Trim(),
                        poolLength,
                        selectedLanes
                    );

                    // Set optional AddressId
                    if (!string.IsNullOrWhiteSpace(txtAddressId.Text) && int.TryParse(txtAddressId.Text, out int addressId))
                    {
                        swimmingPool.AddressId = addressId;
                    }

                    // Save to JSON file using JsonDataService
                    await _dataService.AddSwimmingPoolAsync(swimmingPool);

                    // Show debug info (remove this in production)
                    MessageBox.Show($"Swimming Pool saved to: {_dataService.GetSwimmingPoolsFilePath()}", 
                                  "Debug Info", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);

                    // Raise the event with the saved swimming pool
                    SwimmingPoolSaveRequested?.Invoke(this, new SwimmingPoolSavedEventArgs(swimmingPool));
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
                    // Re-enable the save button
                    btnSave.IsEnabled = true;
                    btnSave.Content = "Save Swimming Pool";
                }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Pool Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }

            string poolLength = GetComboBoxValue(cmbPoolLength);
            if (string.IsNullOrWhiteSpace(poolLength))
            {
                MessageBox.Show("Pool Length is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbPoolLength.Focus();
                return false;
            }

            if (!decimal.TryParse(poolLength, out decimal lengthValue) || lengthValue <= 0)
            {
                MessageBox.Show("Pool Length must be a valid positive number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbPoolLength.Focus();
                return false;
            }

            if (cmbNumberOfLanes.SelectedItem == null)
            {
                MessageBox.Show("Number of Lanes is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbNumberOfLanes.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtAddressId.Text) && !int.TryParse(txtAddressId.Text, out _))
            {
                MessageBox.Show("Address ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAddressId.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Clear();
            cmbPoolLength.SelectedIndex = -1;
            cmbPoolLength.Text = string.Empty;
            cmbNumberOfLanes.SelectedItem = null;
            txtAddressId.Clear();
            
            txtName.Focus();
        }

        private string GetComboBoxValue(ComboBox comboBox)
        {
            if (comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return selectedItem.Content?.ToString() ?? string.Empty;
            }
            return comboBox.Text ?? string.Empty;
        }

        public NumberOfLanes? SelectedNumberOfLanes => cmbNumberOfLanes.SelectedValue as NumberOfLanes?;
    }
}