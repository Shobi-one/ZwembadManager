using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Classes
{
    public class User
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string License { get; set; }
        public string County { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string KeycloakGroups { get; set; }
        public bool IsActive { get; set; }


        public override string ToString() => $"User: {FirstName} {LastName} ({Email})";
    }
}
