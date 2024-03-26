using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticePatch
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
