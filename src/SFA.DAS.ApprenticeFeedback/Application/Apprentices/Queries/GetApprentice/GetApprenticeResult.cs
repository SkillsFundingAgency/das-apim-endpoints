using System;

namespace SFA.DAS.ApprenticeFeedback.Application.Apprentices.Queries.GetApprentice
{
    public class GetApprenticeResult
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
