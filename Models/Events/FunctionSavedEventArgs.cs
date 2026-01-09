using System;
using ZwembaadManager.Models;

namespace ZwembaadManager.Events
{
    public class FunctionSavedEventArgs : EventArgs
    {
        public Function SavedFunction { get; }

        public FunctionSavedEventArgs(Function savedFunction)
        {
            SavedFunction = savedFunction ?? throw new ArgumentNullException(nameof(savedFunction));
        }
    }
}