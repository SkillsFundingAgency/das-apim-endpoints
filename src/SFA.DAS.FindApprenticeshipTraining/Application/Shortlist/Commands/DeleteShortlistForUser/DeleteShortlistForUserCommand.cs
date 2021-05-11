using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser
{
    public class DeleteShortlistForUserCommand : IRequest<Unit>
    {
        public Guid UserId { get ; set ; }
    }
}