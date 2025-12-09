using System;
using System.Windows;
using System.Windows.Controls;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateFunctionAssignmentView.xaml
    /// </summary>
    public partial class CreateFunctionAssignmentView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? FunctionAssignmentSaveRequested;

        public CreateFunctionAssignmentView()
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
                FunctionAssignmentSaveRequested?.Invoke(this, EventArgs.Empty);
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

            if (!dpAssignmentDate.SelectedDate.HasValue)
            {
                MessageBox.Show("Assignment Date is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                dpAssignmentDate.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtUserId.Clear();
            txtFunctionId.Clear();
            dpAssignmentDate.SelectedDate = DateTime.Today;
            txtNotes.Clear();
            
            txtUserId.Focus();
        }
    }
}