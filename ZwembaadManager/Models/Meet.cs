using System;
using System.Collections.Generic;
using System.Text;

namespace ZwembaadManager.Classes
{
    public class Meet
    {
        public int Id { get; set; }
        public int ClubId { get; set; }
        public int SwimmingPoolId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public string PartOfTheDay { get; set; }
        public string TimeRegistration { get; set; }
        public string Remarks { get; set; }
        public int NumberOfInternships { get; set; }
        public int NumberOfExams { get; set; }
        public string MeetState { get; set; }


        public override string ToString() => $"Meet: {Name} on {Date.ToShortDateString()}";
    }
}
