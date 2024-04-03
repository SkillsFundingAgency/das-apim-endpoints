using MediatR;

namespace SFA.DAS.SharedOuterApi.Employer.GovUK.Auth.Application.Queries.EmployerAccounts
{
    public class GetAccountsQuery : IRequest<GetAccountsQueryResult>
    {
        public string Email { get; set; }
        public string UserId { get; set; }
    }
}