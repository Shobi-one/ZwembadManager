using System;

namespace ZwembaadManager.Models
{
    public class Function
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Function()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public Function(string name, string abbreviation) : this()
        {
            Name = name;
            Abbreviation = abbreviation;
        }

        public override string ToString() => $"Function: {Name} ({Abbreviation})";
    }
}
