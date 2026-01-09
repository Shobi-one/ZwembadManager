using System;

namespace ZwembaadManager.Classes
{
    public class Address
    {
        public Guid Id { get; set; }
        public string Street { get; set; } = string.Empty;
        public string HouseNumber { get; set; } = string.Empty;
        public string ZipCodeCity { get; set; } = string.Empty;

        public Address()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString() => $"Address: {Street} {HouseNumber}, {ZipCodeCity}";
    }
}