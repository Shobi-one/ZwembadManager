using System;
using System.Windows;
using System.Windows.Controls;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateJurysMemberView.xaml
    /// </summary>
    public partial class CreateJurysMemberView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? JurysMemberSaveRequested;

        public CreateJurysMemberView()
        {
            InitializeComponent();
            dpAssignmentDate.SelectedDate = DateTime.Today;
        }

        private void BtnBackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackToDashboardRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                JurysMemberSaveRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtOfficialId.Text))
            {
                MessageBox.Show("Official ID is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOfficialId.Focus();
                return false;
            }

            if (!int.TryParse(txtOfficialId.Text, out _))
            {
                MessageBox.Show("Official ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOfficialId.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtMeetId.Text))
            {
                MessageBox.Show("Meet ID is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMeetId.Focus();
                return false;
            }

            if (!int.TryParse(txtMeetId.Text, out _))
            {
                MessageBox.Show("Meet ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtMeetId.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtOfficialId.Clear();
            txtMeetId.Clear();
            cmbRole.SelectedIndex = -1;
            dpAssignmentDate.SelectedDate = DateTime.Today;
            txtNotes.Clear();
            
            txtOfficialId.Focus();
        }
    }
}