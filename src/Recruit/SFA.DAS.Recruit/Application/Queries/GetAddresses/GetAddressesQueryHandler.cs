using MediatR;
using SFA.DAS.Recruit.Configuration;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.Recruit.Application.Queries.GetAddresses
{
    public class GetAddressesQueryHandler(
        ILocationApiClient<LocationApiConfiguration> locationApiClient,
        RecruitConfiguration config)
        : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
    {
        public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrEmpty(request.Query);

            var addressesResponse = await locationApiClient.Get<GetAddressesListResponse>(
                new GetAddressesQueryRequest(request.Query, config.LocationsApiMinMatch));

            // Filter addresses to only include those in England
            var englandAddresses = addressesResponse?.Addresses?
                .Where(x => string.Equals(x.Country, nameof(Country.England), StringComparison.OrdinalIgnoreCase))
                .ToList() ?? [];

            return new GetAddressesQueryResult(new GetAddressesListResponse { Addresses = englandAddresses });
        }
    }
}