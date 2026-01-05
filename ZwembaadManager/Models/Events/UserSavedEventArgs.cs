using System;
using ZwembaadManager.Classes;

namespace ZwembaadManager.Events
{
    public class UserSavedEventArgs : EventArgs
    {
        public User SavedUser { get; }

        public UserSavedEventArgs(User savedUser)
        {
            SavedUser = savedUser ?? throw new ArgumentNullException(nameof(savedUser));
        }
    }
}