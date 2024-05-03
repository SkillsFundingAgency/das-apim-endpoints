using System;
using System.Collections.Generic;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.CandidatePreferences;
public class UpsertCandidatePreferencesCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public List<CandidatePreference> CandidatePreferences { get; set; }

    public class CandidatePreference
    {
        public Guid PreferenceId { get; set; }
        public string PreferenceMeaning { get; set; } = null!;
        public string PreferenceHint { get; set; } = null!;
        public List<ContactMethodStatus> ContactMethodsAndStatus { get; set; }
    }

    public class ContactMethodStatus
    {
        public string ContactMethod { get; set; }
        public bool Status { get; set; }
    }
}
