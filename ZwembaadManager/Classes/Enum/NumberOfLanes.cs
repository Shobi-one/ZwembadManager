using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZwembaadManager.Classes.Enum
{
    public enum NumberOfLanes
    {
        [Description("4")]
        Four,
        [Description("5")]
        Five,
        [Description("6")]
        Six,
        [Description("8")]
        Eight,
        [Description("10")]
        Ten
    }
}
