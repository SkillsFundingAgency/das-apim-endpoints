using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeCommitments.Apis.InnerApi
{
    public class VerifyRegistrationRequest : IPostApiRequest
    {
        public string PostUrl => "/registrations";

        public object Data { get; set; }
    }

    public class VerifyRegistrationRequestData
    {
        public Guid RegistrationId { get; set; }
        public Guid UserIdentityId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
    }

    public class VerifyRegistrationResponse
    {
    }
}