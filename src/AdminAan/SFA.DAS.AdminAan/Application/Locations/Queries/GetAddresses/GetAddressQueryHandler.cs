using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Text.RegularExpressions;

namespace SFA.DAS.AdminAan.Application.Locations.Queries.GetAddresses;
public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
{
    private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
    public const double MinimumMatch = 0.1;
    public const double MaximumMatch = 1;
    public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";

    public GetAddressesQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient) => _locationApiClient = locationApiClient;

    public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var isFullPostcode = Regex.IsMatch(request.Query, PostcodeRegex, RegexOptions.None, TimeSpan.FromSeconds(5));
        var minMatch = isFullPostcode ? MaximumMatch : MinimumMatch;
        var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(request.Query, minMatch));

        var addresses = addressesResponse.Addresses.Select(a => (AddressItem)a);

        return new GetAddressesQueryResult { Addresses = addresses };
    }
}