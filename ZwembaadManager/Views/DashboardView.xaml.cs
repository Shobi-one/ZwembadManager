using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZwembaadManager.Views
{
    /// <summary>
    /// Interaction logic for DashboardView.xaml
    /// </summary>
    public partial class DashboardView : UserControl
    {
        // Events to communicate with parent window
        public event EventHandler<string>? CreateEntityRequested;
        public event EventHandler? LogoutRequested;
        public event EventHandler? RefreshRequested;

        public DashboardView()
        {
            InitializeComponent();
        }

        // Core Entities Event Handlers
        private void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "User");
        }

        private void BtnCreateClub_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "Club");
        }

        private void BtnCreateMeet_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "Meet");
        }

        private void BtnCreateSwimmingPool_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "SwimmingPool");
        }

        private void BtnCreateAddress_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "Address");
        }

        // Functions Event Handlers
        private void BtnCreateFunction_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "Function");
        }

        private void BtnCreateFunctionAssignment_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "FunctionAssignment");
        }

        private void BtnCreateUsersFunction_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "UsersFunction");
        }

        private void BtnCreateMeetFunction_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "MeetFunction");
        }

        // Jury Management Event Handlers
        private void BtnCreateJurysMember_Click(object sender, RoutedEventArgs e)
        {
            CreateEntityRequested?.Invoke(this, "JurysMember");
        }

        // Action Event Handlers
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshRequested?.Invoke(this, EventArgs.Empty);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            LogoutRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}