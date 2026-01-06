using System;

namespace ZwembaadManager.Models
{
    public class Meet
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string PartOfTheDay { get; set; } = string.Empty;
        public string MeetState { get; set; } = string.Empty;
        public Guid? ClubId { get; set; }
        public Guid? SwimmingPoolId { get; set; }
        public string TimeRegistration { get; set; } = string.Empty;
        public int? NumberOfInternships { get; set; }
        public int? NumberOfExams { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Meet()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
            Date = DateTime.Today;
        }

        public Meet(string name, DateTime date, string partOfTheDay, string meetState) : this()
        {
            Name = name;
            Date = date;
            PartOfTheDay = partOfTheDay;
            MeetState = meetState;
        }

        public override string ToString() => $"Meet: {Name} on {Date:yyyy-MM-dd} ({PartOfTheDay})";
    }
}
