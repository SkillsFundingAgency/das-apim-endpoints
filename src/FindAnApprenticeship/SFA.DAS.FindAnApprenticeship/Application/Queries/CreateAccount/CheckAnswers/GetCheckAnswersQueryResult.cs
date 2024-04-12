using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.CheckAnswers;

public class GetCheckAnswersQueryResult
{
    public string FirstName { get; set; }
    public string MiddleNames { get; set; }
    public string LastName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Uprn { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string Town { get; set; }
    public string County { get; set; }
    public string Postcode { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public List<CandidatePreference> CandidatePreferences { get; set; }

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