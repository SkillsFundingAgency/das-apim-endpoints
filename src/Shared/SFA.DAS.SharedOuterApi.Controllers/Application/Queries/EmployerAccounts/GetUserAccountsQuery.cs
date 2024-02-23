using MediatR;

namespace SFA.DAS.SharedOuterApi.Controllers.Application.Queries.EmployerAccounts
{
    public class GetUserAccountsQuery : IRequest<GetUserAccountsQueryResult>
    {
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}