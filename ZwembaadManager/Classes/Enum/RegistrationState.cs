using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZwembaadManager.Classes.Enum
{
    public enum RegistrationState
    {
        [Description("Ik ben al official")]
        Officials,
        [Description("Ik wil official worden")]
        Kandidaten,
        [Description("Ik ben kandidaat-official (geslaagd theorie)")]
        Stagiairs,
        [Description("Ik ben sportsecretaris")]
        Sportsecretaris
    }
}
