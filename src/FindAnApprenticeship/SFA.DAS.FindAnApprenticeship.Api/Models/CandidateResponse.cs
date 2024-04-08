using System;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;

namespace SFA.DAS.FindAnApprenticeship.Api.Models;

public class CandidateResponse
{
    public static implicit operator CandidateResponse(CreateCandidateCommandResult source)
    {
        return new CandidateResponse
        {
            Email = source.Email,
            GovUkIdentifier = source.GovUkIdentifier,
            Id = source.Id,
            FirstName = source.FirstName,
            LastName = source.LastName,
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