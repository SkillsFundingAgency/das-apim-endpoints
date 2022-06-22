using MediatR;

namespace SFA.DAS.ApimDeveloper.Application.Users.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserQueryResult>
    {
        public string Email { get; set; }
    }
}