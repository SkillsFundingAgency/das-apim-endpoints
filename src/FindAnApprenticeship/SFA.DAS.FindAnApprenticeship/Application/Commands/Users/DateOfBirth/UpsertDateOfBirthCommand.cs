using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DateOfBirth;
public class UpsertDateOfBirthCommand : IRequest<Unit>
{
    public Guid CandidateId { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Email { get; set; }
}
