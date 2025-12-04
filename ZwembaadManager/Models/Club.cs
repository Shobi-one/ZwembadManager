using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Classes
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Abbreviation { get; set; }


        public override string ToString() => $"Club: {Name} ({Abbreviation})";
    }
}
