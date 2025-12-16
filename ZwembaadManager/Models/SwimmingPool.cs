using System;
using ZwembaadManager.Classes.Enum;

namespace ZwembaadManager.Models
{
    public class SwimmingPool
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal PoolLength { get; set; }
        public NumberOfLanes NumberOfLanes { get; set; }
        public int? AddressId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public SwimmingPool()
        {
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        public SwimmingPool(string name, decimal poolLength, NumberOfLanes numberOfLanes) : this()
        {
            Name = name;
            PoolLength = poolLength;
            NumberOfLanes = numberOfLanes;
        }

        public override string ToString() => $"SwimmingPool: {Name}, {PoolLength}m, {NumberOfLanes} lanes";
    }
}
