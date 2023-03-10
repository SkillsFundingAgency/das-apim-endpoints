using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.Providers.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderResult>
    {
        private readonly IRoatpServiceApiClient<RoatpConfiguration> _roatpApiClient;
  
        public GetProviderQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> roatpApiClient)
        {
            _roatpApiClient = roatpApiClient;
        }

        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            GetProviderResponse provider=null;

            var result = await _roatpApiClient.Get<GetRoatpProviderResponse>(
                new GetProviderRequest
                {
                    Ukprn = request.Ukprn
                });

            provider = result;
            
            return new GetProviderResult
            {
                Provider = provider
            };
        }
    }
}