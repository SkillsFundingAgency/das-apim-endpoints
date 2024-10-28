namespace SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Models;

public class Apprentice
{
    public Guid ApprenticeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool TermsOfUseAccepted { get; set; }
    public string GovUkIdentifier { get; set; }
}