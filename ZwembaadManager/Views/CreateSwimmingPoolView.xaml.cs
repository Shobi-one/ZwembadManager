using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Classes.Enum;
using ZwembaadManager.Extensions;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateSwimmingPoolView.xaml
    /// </summary>
    public partial class CreateSwimmingPoolView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? SwimmingPoolSaveRequested;

        public CreateSwimmingPoolView()
        {
            InitializeComponent();
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                SwimmingPoolSaveRequested?.Invoke(this, EventArgs.Empty);
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