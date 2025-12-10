using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Classes
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCodeCity { get; set; }


        public override string ToString() => $"Address: {Street} {HouseNumber}, {ZipCodeCity}";
    }
}
