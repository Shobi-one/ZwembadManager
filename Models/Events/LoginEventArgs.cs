using System;

namespace ZwembaadManager.Events
{
    public class LoginEventArgs : EventArgs
    {
        public string Username { get; set; } = string.Empty;
    }
}