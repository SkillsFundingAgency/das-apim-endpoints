using System;
using System.Collections.Generic;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class CandidatesModel
{
    public string Email { get; set; } = null!;
}

public class CandidatesNameModel : CandidatesModel
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
}

public class CandidatesDateOfBirthModel : CandidatesModel
{
    public DateTime DateOfBirth { get; set; }
}

public class CandidatesAddressModel : CandidatesModel
{
    public string? Uprn { get; set; }
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Postcode { get; set; }
}

public class CandidatesManuallyEnteredAddressModel : CandidatesModel
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string TownOrCity { get; set; }
    public string County { get; set; }
    public string Postcode { get; set; }
}

public class CandidatePreferencesModel : CandidatesModel
{
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

public class CandidatesPhoneNumberModel : CandidatesModel
{
    public string PhoneNumber { get; set; }
}