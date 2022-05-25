using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses
{
    public class GetApprenticeResponse
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool TermsOfUseAccepted { get; set; }
        public bool IsPrivateBetaUser { get; set; }
        public bool ReacceptTermsOfUseRequired { get; set; }

    }
}
