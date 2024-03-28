using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class CandidateResponse
{
    public static implicit operator CandidateResponse(PutCandidateCommandResult source)
    {
        return new CandidateResponse
        {
            GovUkIdentifier = source.GovUkIdentifier,
            Email = source.Email,
            FirstName = source.FirstName,
            LastName = source.LastName,
            Id = source.Id,
            PhoneNumber = source.PhoneNumber
        };
    }
    public Guid Id { get; set; }
    public string GovUkIdentifier { get; set; }
    public string Email { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
}