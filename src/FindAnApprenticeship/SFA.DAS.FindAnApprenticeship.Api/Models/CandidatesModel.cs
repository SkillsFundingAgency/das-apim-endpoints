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