using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetProviders
{
    public class GetProvidersQueryHandler : IRequestHandler<GetProvidersQuery, GetProvidersQueryResult>
    {
        private readonly IProviderCoursesApiClient<ProviderCoursesApiConfiguration> _roatpServiceApiClient;

        public GetProvidersQueryHandler ( IProviderCoursesApiClient<ProviderCoursesApiConfiguration> roatpServiceApiClient)
        {
            _roatpServiceApiClient = roatpServiceApiClient;
        }
        public async Task<GetProvidersQueryResult> Handle(GetProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = await _roatpServiceApiClient.Get<GetProvidersListResponse>(new GetProvidersRequest());
            
            return new GetProvidersQueryResult
            {
                Providers = response.RegisteredProviders
            };
        }
    }
}