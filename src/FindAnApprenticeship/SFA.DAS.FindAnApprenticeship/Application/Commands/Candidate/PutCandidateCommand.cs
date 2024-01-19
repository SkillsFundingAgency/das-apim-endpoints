#nullable enable
using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
public class PutCandidateCommand : IRequest<PutCandidateCommandResult>
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string GovUkIdentifier { get; set; } = null!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}
