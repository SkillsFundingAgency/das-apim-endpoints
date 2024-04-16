using System;
using SFA.DAS.FindAnApprenticeship.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class GetCandidateApiResponse
    {
        public Guid Id { get; set; }
        public string GovUkIdentifier { get; set; }
        public string FirstName { get; set; }
        public string MiddleNames { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
    }
}
