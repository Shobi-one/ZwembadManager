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
    /// Interaction logic for CreateUserView.xaml
    /// </summary>
    public partial class CreateUserView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? UserSaveRequested;

        public CreateUserView()
        {
            InitializeComponent();
            LoadGenderOptions();
        }

        private void LoadGenderOptions()
        {
            var genderOptions = Enum.GetValues<Gender>()
                .Select(g => new { DisplayName = g.GetDisplayName(), Value = g })
                .ToList();

            cmbGender.ItemsSource = genderOptions;
        }

        private void BtnBackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackToDashboardRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                UserSaveRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFirstName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtLastName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Email is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtEmail.Focus();
                return false;
            }

            if (cmbGender.SelectedItem == null)
            {
                MessageBox.Show("Gender is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbGender.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtClubId.Text) && !int.TryParse(txtClubId.Text, out _))
            {
                MessageBox.Show("Club ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtClubId.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtFirstName.Clear();
            txtLastName.Clear();
            txtEmail.Clear();
            txtMobile.Clear();
            txtClubId.Clear();
            txtLicense.Clear();
            txtCounty.Clear();
            cmbGender.SelectedItem = null;
            
            txtFirstName.Focus();
        }

        public Gender? SelectedGender => cmbGender.SelectedValue as Gender?;
    }
}