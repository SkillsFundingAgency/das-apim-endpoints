using System;
using System.Collections.Generic;

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

        public List<ApprenticePreferences> ApprenticePreferences { get; set; } = new List<ApprenticePreferences>();
    }

    public class ApprenticePreferences
    {
        public int PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; }
        public bool Enabled { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
