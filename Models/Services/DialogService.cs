using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ZwembaadManager.Services
{
    public class DialogService : IDialogService
    {
        public void ShowInformation(string message, string title = "Information")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWarning(string message, string title = "Warning")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void ShowError(string message, string title = "Error")
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public DialogResult ShowConfirmation(string message, string title = "Confirmation")
        {
            var result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question);
            return result == MessageBoxResult.Yes ? DialogResult.Yes : DialogResult.No;
        }
    }
}
