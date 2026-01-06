using System;
using ZwembaadManager.Models;

namespace ZwembaadManager.Events
{
    public class ClubSavedEventArgs : EventArgs
    {
        public Club SavedClub { get; }

        public ClubSavedEventArgs(Club savedClub)
        {
            SavedClub = savedClub ?? throw new ArgumentNullException(nameof(savedClub));
        }
    }
}