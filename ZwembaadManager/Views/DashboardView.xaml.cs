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
        public event EventHandler<string>? OpenViewRequested;
        public event EventHandler? LogoutRequested;
        public event EventHandler? RefreshRequested;

        public DashboardView()
        {
            InitializeComponent();
        }

        private void BtnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateUser");
        }

        private void BtnCreateClub_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateClub");
        }

        private void BtnCreateMeet_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateMeet");
        }

        private void BtnCreateSwimmingPool_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateSwimmingPool");
        }

        private void BtnCreateAddress_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateAddress");
        }

        private void BtnCreateFunction_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateFunction");
        }

        private void BtnCreateFunctionAssignment_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateFunctionAssignment");
        }

        private void BtnCreateUsersFunction_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateUsersFunction");
        }

        private void BtnCreateMeetFunction_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateMeetFunction");
        }

        private void BtnCreateJurysMember_Click(object sender, RoutedEventArgs e)
        {
            OpenViewRequested?.Invoke(this, "CreateJurysMember");
        }

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