using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.Providers.Queries.GetProvider
{
    public class GetProviderQueryHandler(IRoatpServiceApiClient<RoatpConfiguration> roatpApiClient)
        : IRequestHandler<GetProviderQuery, GetProviderResult>
    {
        public async Task<GetProviderResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var result = await roatpApiClient.Get<GetRoatpProviderResponse>(
                new GetProviderRequest
                {
                    Ukprn = request.Ukprn
                });

            return new GetProviderResult
            {
                Provider = result
            };
        }
    }
}