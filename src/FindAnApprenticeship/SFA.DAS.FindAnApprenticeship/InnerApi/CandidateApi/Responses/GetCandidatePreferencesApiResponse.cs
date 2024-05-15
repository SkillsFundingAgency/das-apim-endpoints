using System.Collections.Generic;
using System;

namespace SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
public class GetCandidatePreferencesApiResponse
{
    public List<CandidatePreference> CandidatePreferences { get; set; }

    public class CandidatePreference()
    {
        public Guid PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; }
        public string PreferenceHint { get; set; }
        public List<ContactMethodStatus> ContactMethodsAndStatus { get; set; }
    }

    public class ContactMethodStatus()
    {
        public string? ContactMethod { get; set; }
        public bool? Status { get; set; }
    }
}
