using System;
using System.Windows;
using System.Windows.Controls;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateMeetView.xaml
    /// </summary>
    public partial class CreateMeetView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? MeetSaveRequested;

        public CreateMeetView()
        {
            InitializeComponent();
            
            dpDate.SelectedDate = DateTime.Today;
            cmbMeetState.SelectedIndex = 0;
        }

        private void BtnBackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackToDashboardRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                MeetSaveRequested?.Invoke(this, EventArgs.Empty);
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