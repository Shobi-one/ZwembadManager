using System;

namespace ZwembaadManager.Classes
{
    public class MeetFunction
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Order { get; set; }

        public MeetFunction()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString() => $"MeetFunction: {Name} (Order {Order})";
    }
}
