using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class VerifyRegistrationRequest : IPostApiRequest<VerifyRegistrationRequestData>
    {
        public string PostUrl => "/registrations";

        public VerifyRegistrationRequestData Data { get; set; }
    }

    public class VerifyRegistrationRequestData
    {
        public Guid ApprenticeId { get; set; }
        public Guid UserIdentityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}