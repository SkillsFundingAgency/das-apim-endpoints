#nullable enable
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
public class PutCandidateCommandResult
{
    public Guid CandidateId { get; set; }
    public string GovUkIdentifier { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
