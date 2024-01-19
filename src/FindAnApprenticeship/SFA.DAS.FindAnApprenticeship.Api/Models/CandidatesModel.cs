#nullable enable
namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class CandidatesModel
{
    public string Email { get; set; } = null!;
    public string GovUkIdentifier { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
