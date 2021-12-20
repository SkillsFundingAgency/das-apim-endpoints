using System;
using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.ActivateUser
{
    public class ActivateUserCommand : IRequest<Unit>
    {
        public Guid Id { get ; set ; }
    }
}