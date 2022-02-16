using System;

namespace SFA.DAS.ApprenticeFeedback.Api.ApiRequests
{
    public class CreateApprentice
    {
        public Guid ApprenticeId { get; set; }
        public Guid ApprenticeshipId { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
    }
}
