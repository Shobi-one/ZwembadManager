using System;

namespace ZwembaadManager.Classes
{
    public class JurysMember
    {
        public Guid Id { get; set; }
        public Guid OfficialId { get; set; }
        public Guid MeetId { get; set; }

        public JurysMember()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString() => $"JurysMember: Official {OfficialId}, Meet {MeetId}";
    }
}
