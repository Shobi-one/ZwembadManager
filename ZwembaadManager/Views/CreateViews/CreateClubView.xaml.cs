using System;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Events;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateClubView.xaml
    /// </summary>
    public partial class CreateClubView : UserControl
    {
        private readonly JsonDataService _dataService;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<ClubSavedEventArgs>? ClubSaveRequested;

        public CreateClubView()
        {
            InitializeComponent();
            _dataService = new JsonDataService();
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

                    // Create new club object
                    var club = new Club(
                        txtName.Text.Trim(),
                        txtAbbreviation.Text.Trim()
                    );

                    // Save to JSON file using JsonDataService (Resources folder)
                    await _dataService.AddClubAsync(club);

                    // Show the file path for debugging
                    MessageBox.Show($"Club saved to: {_dataService.GetClubsFilePath()}", 
                                  "Debug Info", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);

                    // Raise the event with the saved club
                    ClubSaveRequested?.Invoke(this, new ClubSavedEventArgs(club));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving club: {ex.Message}", 
                                  "Save Error", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Error);
                }
                finally
                {
                    // Re-enable the save button
                    btnSave.IsEnabled = true;
                    btnSave.Content = "Save Club";
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
                MessageBox.Show("Club Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtAbbreviation.Text))
            {
                MessageBox.Show("Abbreviation is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAbbreviation.Focus();
                return false;
            }

            if (txtAbbreviation.Text.Trim().Length > 10)
            {
                MessageBox.Show("Abbreviation should not exceed 10 characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAbbreviation.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtAbbreviation.Clear();
            txtName.Focus();
        }
    }
}