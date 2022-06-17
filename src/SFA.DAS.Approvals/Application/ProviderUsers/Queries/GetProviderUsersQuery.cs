using MediatR;

namespace SFA.DAS.Approvals.Application.ProviderUsers.Queries
{
    public class GetProviderUsersQuery : IRequest<GetProviderUsersQueryResult>
    {
        public long Ukprn { get; set; }
    }
}