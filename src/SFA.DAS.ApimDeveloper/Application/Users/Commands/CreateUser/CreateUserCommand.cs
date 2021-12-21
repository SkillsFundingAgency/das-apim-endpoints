using System;
using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<Unit>
    {
        public string Email { get ; set ; }
        public string Password { get ; set ; }
        public string FirstName { get ; set ; }
        public string LastName { get ; set ; }
        public string ConfirmationEmailLink { get ; set ; }
        public Guid Id { get ; set ; }
    }
}