using System;
using System.Windows;
using System.Windows.Controls;
using ZwembaadManager.Events;
using ZwembaadManager.Models;
using ZwembaadManager.Services;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateMeetView.xaml
    /// </summary>
    public partial class CreateMeetView : UserControl
    {
        private readonly JsonDataService _dataService;

        public event EventHandler? BackToDashboardRequested;
        public event EventHandler<MeetSavedEventArgs>? MeetSaveRequested;

        public CreateMeetView()
        {
            InitializeComponent();
            _dataService = new JsonDataService();
            
            dpDate.SelectedDate = DateTime.Today;
            cmbMeetState.SelectedIndex = 0;
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

                    // Create new meet object
                    var meet = new Meet(
                        txtName.Text.Trim(),
                        dpDate.SelectedDate!.Value,
                        ((ComboBoxItem)cmbPartOfTheDay.SelectedItem).Content.ToString()!,
                        ((ComboBoxItem)cmbMeetState.SelectedItem).Content.ToString()!
                    );

                    // Set optional properties
                    if (!string.IsNullOrWhiteSpace(txtClubId.Text) && int.TryParse(txtClubId.Text, out int clubId))
                    {
                        meet.ClubId = clubId;
                    }

                    if (!string.IsNullOrWhiteSpace(txtSwimmingPoolId.Text) && int.TryParse(txtSwimmingPoolId.Text, out int poolId))
                    {
                        meet.SwimmingPoolId = poolId;
                    }

                    meet.TimeRegistration = txtTimeRegistration.Text.Trim();

                    if (!string.IsNullOrWhiteSpace(txtNumberOfInternships.Text) && int.TryParse(txtNumberOfInternships.Text, out int internships))
                    {
                        meet.NumberOfInternships = internships;
                    }

                    if (!string.IsNullOrWhiteSpace(txtNumberOfExams.Text) && int.TryParse(txtNumberOfExams.Text, out int exams))
                    {
                        meet.NumberOfExams = exams;
                    }

                    meet.Remarks = txtRemarks.Text.Trim();

                    // Save to JSON file using JsonDataService
                    await _dataService.AddMeetAsync(meet);

                    // Show debug info (remove this in production)
                    MessageBox.Show($"Meet saved to: {_dataService.GetMeetsFilePath()}", 
                                  "Debug Info", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Information);

                    // Raise the event with the saved meet
                    MeetSaveRequested?.Invoke(this, new MeetSavedEventArgs(meet));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving meet: {ex.Message}", 
                                  "Save Error", 
                                  MessageBoxButton.OK, 
                                  MessageBoxImage.Error);
                }
                finally
                {
                    // Re-enable the save button
                    btnSave.IsEnabled = true;
                    btnSave.Content = "Save Meet";
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
                MessageBox.Show("Meet Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }

            if (!dpDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpDate.Focus();
                return false;
            }

            if (cmbPartOfTheDay.SelectedItem == null)
            {
                MessageBox.Show("Please select a part of the day.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbPartOfTheDay.Focus();
                return false;
            }

            if (cmbMeetState.SelectedItem == null)
            {
                MessageBox.Show("Please select a meet state.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbMeetState.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtClubId.Text) && !int.TryParse(txtClubId.Text, out _))
            {
                MessageBox.Show("Club ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtClubId.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtSwimmingPoolId.Text) && !int.TryParse(txtSwimmingPoolId.Text, out _))
            {
                MessageBox.Show("Swimming Pool ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtSwimmingPoolId.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtNumberOfInternships.Text) && !int.TryParse(txtNumberOfInternships.Text, out _))
            {
                MessageBox.Show("Number of Internships must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNumberOfInternships.Focus();
                return false;
            }

            if (!string.IsNullOrWhiteSpace(txtNumberOfExams.Text) && !int.TryParse(txtNumberOfExams.Text, out _))
            {
                MessageBox.Show("Number of Exams must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtNumberOfExams.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Clear();
            dpDate.SelectedDate = DateTime.Today;
            cmbPartOfTheDay.SelectedIndex = -1;
            cmbMeetState.SelectedIndex = 0;
            txtClubId.Clear();
            txtSwimmingPoolId.Clear();
            txtTimeRegistration.Clear();
            txtNumberOfInternships.Clear();
            txtNumberOfExams.Clear();
            txtRemarks.Clear();
            
            txtName.Focus();
        }

        private void SetComboBoxSelection(ComboBox comboBox, string value)
        {
            if (string.IsNullOrEmpty(value)) return;

            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content?.ToString() == value)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }
}