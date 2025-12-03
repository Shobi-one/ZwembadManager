using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZwembaadManager.Classes.Enum
{
    public enum PartOfTheDay
    {
        [Description("Voormiddag")]
        VM,
        [Description("Namiddag")]
        NM,
        [Description("Avond")]
        AV,
        [Description("1 Dag")]
        D1
    }
}
