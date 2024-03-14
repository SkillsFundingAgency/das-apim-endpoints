using System;
using MediatR;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DateOfBirth;
public class UpsertDateOfBirthCommand : IRequest<Unit>
{
    public string GovUkIdentifier { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; }
}
