using System;
using MediatR;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser
{
    public class DeleteShortlistItemForUserCommand : IRequest<Unit>
    {
        public Guid UserId { get ; set ; }
        public Guid Id { get ; set ; }
    }
}