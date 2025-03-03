using MediatR;
using System;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteCandidate
{
    public record DeleteCandidateCommand(Guid CandidateId) : IRequest<Unit>;
}