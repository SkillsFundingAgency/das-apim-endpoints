using MediatR;

namespace SFA.DAS.Assessor.Application.Queries.GetUser
{
    public class GetUserQuery : IRequest<GetUserResult>
    {
        public required string DfeSignInUserId { get; set; }
    }
}