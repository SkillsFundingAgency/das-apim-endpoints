using MediatR;
using SFA.DAS.Assessors.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessors.Application.Queries.GetAddresses
{
    public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
        private readonly AssessorsConfiguration _config;

        public GetAddressesQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient, AssessorsConfiguration config)
        {
            _locationApiClient = locationApiClient;
            _config = config;
        }

        public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Query)) throw new ArgumentException($"Query is required", nameof(GetAddressesQuery.Query));

            var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(request.Query, _config.LocationsApiMinMatch));

            return new GetAddressesQueryResult(addressesResponse);
        }
    }
}
