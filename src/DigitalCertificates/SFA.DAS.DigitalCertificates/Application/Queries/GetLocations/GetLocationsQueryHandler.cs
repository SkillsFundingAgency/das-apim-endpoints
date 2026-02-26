using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.DigitalCertificates.Application.Queries.GetLocations
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, GetLocationsResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
        private readonly Configuration.DigitalCertificatesConfiguration _configuration;

        public GetLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient, Configuration.DigitalCertificatesConfiguration configuration)
        {
            _locationApiClient = locationApiClient;
            _configuration = configuration;
        }

        public async Task<GetLocationsResult> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var minMatch = _configuration?.MinMatch ?? 0.4;

            var response = await _locationApiClient.GetWithResponseCode<GetAddressesListResponse>(new GetAddressesQueryRequest(request.Query, minMatch));

            if (response == null || response.StatusCode == HttpStatusCode.NotFound)
            {
                return new GetLocationsResult { Addresses = null };
            }

            response.EnsureSuccessStatusCode();

            return new GetLocationsResult { Addresses = response.Body };
        }
    }
}
