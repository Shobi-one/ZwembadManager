using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Classes
{
    public class JurysMember
    {
        public int Id { get; set; }
        public int OfficialId { get; set; }
        public int MeetId { get; set; }


        public override string ToString() => $"JurysMember: Official {OfficialId}, Meet {MeetId}";
    }
}
