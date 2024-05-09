using System.Collections.Generic;
#nullable enable
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePreferences;
public class GetCandidatePreferencesQueryResult
{
    public List<CandidatePreference> CandidatePreferences { get; set; } = new List<CandidatePreference>();

    public class CandidatePreference
    {
        public Guid PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; } = null!;
        public string PreferenceHint { get; set; } = null!;
        public List<ContactMethodStatus>? ContactMethodsAndStatus { get; set; }
    }

    public class ContactMethodStatus
    {
        public string? ContactMethod { get; set; }
        public bool? Status { get; set; }
    }
}
