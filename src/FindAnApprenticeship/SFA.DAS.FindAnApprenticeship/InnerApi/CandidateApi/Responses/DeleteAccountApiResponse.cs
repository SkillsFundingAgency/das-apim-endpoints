using System;
using SFA.DAS.FindAnApprenticeship.Domain.Models;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses
{
    public class DeleteAccountApiResponse
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
        public GetAddressApiResponse Address { get; set; }
        public string MigratedEmail { get; set; }
    }
}
