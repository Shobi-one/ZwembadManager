using System;
using System.Windows;
using System.Windows.Controls;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for CreateClubView.xaml
    /// </summary>
    public partial class CreateClubView : UserControl
    {
        public event EventHandler? BackToDashboardRequested;
        public event EventHandler? ClubSaveRequested;

        public CreateClubView()
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
                ClubSaveRequested?.Invoke(this, EventArgs.Empty);
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