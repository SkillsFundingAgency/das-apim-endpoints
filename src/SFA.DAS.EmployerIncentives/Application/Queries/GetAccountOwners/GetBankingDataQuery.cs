using MediatR;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetAccountOwners
{
    public class GetAccountOwnersQuery : IRequest<GetAccountOwnersResult>
    {
        public string HashedAccountId { get; set; }
    }
}