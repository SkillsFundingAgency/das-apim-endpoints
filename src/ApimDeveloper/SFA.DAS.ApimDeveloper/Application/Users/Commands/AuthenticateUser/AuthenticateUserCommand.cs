using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.Users.Commands.AuthenticateUser
{
    public class AuthenticateUserCommand : IRequest<AuthenticateUserCommandResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        
    }
}