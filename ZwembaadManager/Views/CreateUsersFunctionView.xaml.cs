using System;
using System.Windows;
using System.Windows.Controls;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateUsersFunctionView.xaml
    /// </summary>
    public partial class CreateUsersFunctionView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? UsersFunctionSaveRequested;

        public CreateUsersFunctionView()
        {
            InitializeComponent();
            dpStartDate.SelectedDate = DateTime.Today;
            cmbStatus.SelectedIndex = 0;
        }

        private void BtnBackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackToDashboardRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                UsersFunctionSaveRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtUserId.Text))
            {
                MessageBox.Show("User ID is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUserId.Focus();
                return false;
            }

            if (!int.TryParse(txtUserId.Text, out _))
            {
                MessageBox.Show("User ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtUserId.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtFunctionId.Text))
            {
                MessageBox.Show("Function ID is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFunctionId.Focus();
                return false;
            }

            if (!int.TryParse(txtFunctionId.Text, out _))
            {
                MessageBox.Show("Function ID must be a valid number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFunctionId.Focus();
                return false;
            }

            if (cmbStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                cmbStatus.Focus();
                return false;
            }

            if (!dpStartDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Start Date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpStartDate.Focus();
                return false;
            }

            if (dpEndDate.SelectedDate.HasValue && dpStartDate.SelectedDate.HasValue)
            {
                if (dpEndDate.SelectedDate.Value < dpStartDate.SelectedDate.Value)
                {
                    MessageBox.Show("End Date cannot be before Start Date.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    dpEndDate.Focus();
                    return false;
                }
            }

            return true;
        }

        private void ClearForm()
        {
            txtUserId.Clear();
            txtFunctionId.Clear();
            cmbStatus.SelectedIndex = 0;
            dpStartDate.SelectedDate = DateTime.Today;
            dpEndDate.SelectedDate = null;
            txtRemarks.Clear();
            
            txtUserId.Focus();
        }
    }
}