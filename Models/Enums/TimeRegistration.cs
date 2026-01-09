using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZwembaadManager.Classes.Enum
{
    public enum TimeRegistration
    {
        Manueel,
        AEI,
        [Description("Manueel/AEI")]
        Onbeslist
    }
}
