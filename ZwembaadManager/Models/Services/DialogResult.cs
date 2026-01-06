using System.Windows;

namespace ZwembaadManager.Services
{
    public enum DialogResult
    {
        None,
        OK,
        Cancel,
        Yes,
        No
    }

    public interface IDialogService
    {
        void ShowInformation(string message, string title = "Information");
        void ShowWarning(string message, string title = "Warning");
        void ShowError(string message, string title = "Error");
        DialogResult ShowConfirmation(string message, string title = "Confirmation");
    }
}