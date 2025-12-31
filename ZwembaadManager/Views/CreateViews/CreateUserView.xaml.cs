using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Classes;
using ZwembaadManager.Classes.Enum;
using ZwembaadManager.Extensions;
using ZwembaadManager.Services;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateUserView.xaml
    /// </summary>
    public partial class CreateUserView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<UserSavedEventArgs>? UserSaveRequested;

        private readonly JsonDataService _dataService;

        public CreateUserView()
        {
            InitializeComponent();
            _dataService = new JsonDataService();
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

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                try
                {

                    btnSave.IsEnabled = false;
                    btnSave.Content = "Saving...";
                    var newUser = CreateUserFromForm();

                    await _dataService.AddUserAsync(newUser);
                    UserSaveRequested?.Invoke(this, new UserSavedEventArgs { SavedUser = newUser });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving user: {ex.Message}", "Save Error", 
                                  MessageBoxButton.OK, MessageBoxImage.Error);

                    btnSave.IsEnabled = true;
                    btnSave.Content = "Save User";
                }
            }
        }

        private User CreateUserFromForm()
        {
            var user = new User
            {
                FirstName = txtFirstName.Text.Trim(),
                LastName = txtLastName.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Mobile = txtMobile.Text.Trim(),
                License = txtLicense.Text.Trim(),
                County = txtCounty.Text.Trim(),
                IsActive = true
            };

            if (!string.IsNullOrWhiteSpace(txtClubId.Text) && int.TryParse(txtClubId.Text, out int clubId))
            {
                user.ClubId = clubId;
            }

            if (SelectedGender.HasValue)
            {
                user.KeycloakGroups = SelectedGender.Value.ToString();
            }

            return user;
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

            btnSave.IsEnabled = true;
            btnSave.Content = "Save User";
            
            txtFirstName.Focus();
        }

        public Gender? SelectedGender => cmbGender.SelectedValue as Gender?;
    }

    public class UserSavedEventArgs : EventArgs
    {
        public User SavedUser { get; set; } = null!;
    }
}