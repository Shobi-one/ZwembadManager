using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Models
{
    public class Club
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Club()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public Club(string name, string abbreviation) : this()
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public override string ToString() => $"Club: {Name} ({Abbreviation})";
    }
}
