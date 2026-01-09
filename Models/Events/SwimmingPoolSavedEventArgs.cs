using System;
using ZwembaadManager.Models;

namespace ZwembaadManager.Events
{
    public class SwimmingPoolSavedEventArgs : EventArgs
    {
        public SwimmingPool SavedSwimmingPool { get; }

        public SwimmingPoolSavedEventArgs(SwimmingPool savedSwimmingPool)
        {
            SavedSwimmingPool = savedSwimmingPool ?? throw new ArgumentNullException(nameof(savedSwimmingPool));
        }
    }
}