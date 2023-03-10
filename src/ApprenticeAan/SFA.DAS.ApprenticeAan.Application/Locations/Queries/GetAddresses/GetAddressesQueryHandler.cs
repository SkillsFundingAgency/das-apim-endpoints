using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;

public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
{
    private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
    private const double minMatch = 0.1;

    public GetAddressesQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient) => _locationApiClient = locationApiClient;

    public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(request.Query, minMatch));

        var addresses = addressesResponse.Addresses.Select(a => (AddressItem)a);

        return new GetAddressesQueryResult { Addresses = addresses };
    }
}