using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZwembaadManager.Classes.Enum
{
    public enum County
    {
        Antwerpen,
        Henegouwen,
        Limburg,
        Luik,
        Luxemburg,
        Namen,
        [Description("Oost-Vlaanderen")]
        OostVlaanderen,
        [Description("Vlaams-Brabant")]
        VlaamsBrabant,
        [Description("Waals-Brabant")]
        WaalsBrabant,
        [Description("West-Vlaanderen")]
        WestVlaanderen
    }
}
