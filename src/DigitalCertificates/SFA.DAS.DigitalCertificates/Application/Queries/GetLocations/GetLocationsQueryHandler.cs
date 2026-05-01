using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

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
            var minMatch = _configuration?.LocationsApiMinMatch ?? 0.4;

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
