using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;

public class GetPostcodeQueryHandler : IRequestHandler<GetPostcodeQuery, GetPostcodeQueryResult?>
{
    private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
    private const int minMatch = 1;

    public GetPostcodeQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient) => _locationApiClient = locationApiClient;

    public async Task<GetPostcodeQueryResult?> Handle(GetPostcodeQuery request, CancellationToken cancellationToken)
    {
        var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(request.PostCode, minMatch));

        GetPostcodeQueryResult? getPostcodeQueryResult = addressesResponse.Addresses.FirstOrDefault(x => x.Latitude != null && x.Longitude != null)!;

        return getPostcodeQueryResult;
    }
}
