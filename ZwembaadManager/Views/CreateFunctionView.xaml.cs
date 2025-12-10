using System;
using System.Windows;
using System.Windows.Controls;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateFunctionView.xaml
    /// </summary>
    public partial class CreateFunctionView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? FunctionSaveRequested;

        public CreateFunctionView()
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
                FunctionSaveRequested?.Invoke(this, EventArgs.Empty);
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