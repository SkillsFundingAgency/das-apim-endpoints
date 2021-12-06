using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.Users.Queries
{
    public class AuthenticateUserQuery : IRequest<AuthenticateUserQueryResult>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        
    }
}