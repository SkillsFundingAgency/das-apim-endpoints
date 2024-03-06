using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetPostcodes;

public class GetPostcodeQueryHandler : IRequestHandler<GetPostcodeQuery, GetPostcodeQueryResult?>
{
    private readonly ILocationApiClient _locationApiClient;
    private const int minMatch = 1;

    public GetPostcodeQueryHandler(ILocationApiClient locationApiClient) => _locationApiClient = locationApiClient;

    public async Task<GetPostcodeQueryResult?> Handle(GetPostcodeQuery request, CancellationToken cancellationToken)
    {
        var addressesResponse = await _locationApiClient.GetAddresses(request.PostCode, minMatch);

        GetPostcodeQueryResult? getPostcodeQueryResult = addressesResponse.Addresses.FirstOrDefault(x => x.Latitude != null && x.Longitude != null)!;

        return getPostcodeQueryResult;
    }
}
