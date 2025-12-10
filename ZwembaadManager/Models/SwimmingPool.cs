using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Classes
{
    public class SwimmingPool
    {
        public int Id { get; set; }
        public int AddressId { get; set; }
        public string Name { get; set; }
        public string PoolLength { get; set; }
        public string NumberOfLanes { get; set; }


        public override string ToString() => $"SwimmingPool: {Name}, {PoolLength}m, {NumberOfLanes} lanes";
    }
}
