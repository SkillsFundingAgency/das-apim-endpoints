using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersResult>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _providerCoursesApiClient;


        public GetProvidersQueryHandler(IProviderCoursesApiClient<ProviderCoursesApiConfiguration> providerCoursesApiClient)
        {
            _providerCoursesApiClient = providerCoursesApiClient;
        }

        public async Task<GetProvidersResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
                var result = await _providerCoursesApiClient.Get<GetRoatpProvidersListResponse>(new GetProvidersRequest());
                var providers = result.RegisteredProviders;

                return new GetProvidersResult
            {
                Providers = providers
            };
        }
    }
}