using System;

namespace ZwembaadManager.Classes
{
    public class User
    {
        public Guid Id { get; set; }
        public Guid? ClubId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string License { get; set; } = string.Empty;
        public string County { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Mobile { get; set; } = string.Empty;
        public string KeycloakGroups { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public User()
        {
            Id = Guid.NewGuid();
        }

        public override string ToString() => $"User: {FirstName} {LastName} ({Email})";
    }
}
