using System;

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

public class CandidatesPhoneNumberModel : CandidatesModel
{
    public string PhoneNumber { get; set; }
}