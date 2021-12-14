using System;

namespace SFA.DAS.ApprenticePortal.Models
{
    public class ApprenticeHomepage
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CourseName { get; set; }
        public string EmployerName { get; set; }
        public bool? ApprenticeshipComplete { get; set; }
    }
}
