using System;
using System.Windows;
using System.Windows.Controls;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateMeetFunctionView.xaml
    /// </summary>
    public partial class CreateMeetFunctionView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? MeetFunctionSaveRequested;

        public CreateMeetFunctionView()
        {
            InitializeComponent();
        }

        private void BtnBackToDashboard_Click(object sender, RoutedEventArgs e)
        {
            BackToDashboardRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateForm())
            {
                MeetFunctionSaveRequested?.Invoke(this, EventArgs.Empty);
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
                MessageBox.Show("Function Name is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtOrder.Text))
            {
                MessageBox.Show("Order is required.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOrder.Focus();
                return false;
            }

            if (!int.TryParse(txtOrder.Text, out int orderValue) || orderValue < 0)
            {
                MessageBox.Show("Order must be a valid non-negative number.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtOrder.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtOrder.Clear();
            cmbCategory.SelectedIndex = -1;
            txtDescription.Clear();
            
            txtName.Focus();
        }
    }
}