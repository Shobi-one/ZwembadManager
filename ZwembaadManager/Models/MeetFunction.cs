using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Classes
{
    public class MeetFunction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }


        public override string ToString() => $"MeetFunction: {Name} (Order {Order})";
    }
}
