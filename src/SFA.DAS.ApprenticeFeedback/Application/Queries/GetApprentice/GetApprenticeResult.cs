using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprentice
{
    public class GetApprenticeResult
    {
        public Guid ApprenticeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool TermsOfUseAccepted { get; set; }
        public bool IsPrivateBetaUser { get; set; }
        public bool ReacceptTermsOfUseRequired { get; set; }
        public List<ApprenticePreferenceDto> ApprenticePreferences { get; set; } = new List<ApprenticePreferenceDto>();
    }
}
