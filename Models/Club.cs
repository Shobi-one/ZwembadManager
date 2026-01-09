using System;

namespace ZwembaadManager.Models
{
    public class Club
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Abbreviation { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Club()
        {
            Id = Guid.NewGuid();
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
