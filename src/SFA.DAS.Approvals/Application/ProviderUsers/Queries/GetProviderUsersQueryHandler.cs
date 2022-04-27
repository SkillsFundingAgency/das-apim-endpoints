using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderUsers.Queries
{
    public class GetProviderUsersQueryHandler : IRequestHandler<GetProviderUsersQuery, GetProviderUsersQueryResult>
    {
        private readonly IProviderAccountApiClient<ProviderAccountApiConfiguration> _providerAccountApiClient;

        public GetProviderUsersQueryHandler(IProviderAccountApiClient<ProviderAccountApiConfiguration> providerAccountApiClient)
        {
            _providerAccountApiClient = providerAccountApiClient;
        }
        public async Task<GetProviderUsersQueryResult> Handle(GetProviderUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _providerAccountApiClient.GetAll<GetProviderUsersListItem>(new GetProviderUsersRequest(request.Ukprn));

            return new GetProviderUsersQueryResult
            {
                Users = users
            };
        }
    }
}