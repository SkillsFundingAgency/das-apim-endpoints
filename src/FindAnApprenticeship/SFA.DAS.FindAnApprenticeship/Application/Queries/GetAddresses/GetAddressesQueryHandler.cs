using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using SFA.DAS.FindAnApprenticeship.Domain.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetAddresses
{
    public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
        private readonly FindAnApprenticeshipConfiguration _config;

        public GetAddressesQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient, FindAnApprenticeshipConfiguration config)
        {
            _locationApiClient = locationApiClient;
            _config = config;
        }

        public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Postcode)) throw new ArgumentException($"Query is required", nameof(GetAddressesQuery.Postcode));

            var postcode = HttpUtility.UrlDecode(request.Postcode);
            var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(postcode, _config.LocationsApiMinMatch));

            return new GetAddressesQueryResult(addressesResponse);
        }
    }
}
