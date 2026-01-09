using System;
using ZwembaadManager.Models;

namespace ZwembaadManager.Events
{
    public class MeetSavedEventArgs : EventArgs
    {
        public Meet SavedMeet { get; }

        public MeetSavedEventArgs(Meet savedMeet)
        {
            SavedMeet = savedMeet ?? throw new ArgumentNullException(nameof(savedMeet));
        }
    }
}