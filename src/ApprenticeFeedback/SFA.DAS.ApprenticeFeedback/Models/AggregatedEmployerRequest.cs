using System;

namespace SFA.DAS.ApprenticeFeedback.Models
{
    public class AggregatedEmployerRequest
    {
        public string CourseReference { get; set; }
        public string CourseTitle { get; set; }
        public int Level { get; set; }
        public string Sector { get; set; }
        public int NumberOfApprentices { get; set; }
        public int NumberOfEmployers { get; set; }
    }
}
