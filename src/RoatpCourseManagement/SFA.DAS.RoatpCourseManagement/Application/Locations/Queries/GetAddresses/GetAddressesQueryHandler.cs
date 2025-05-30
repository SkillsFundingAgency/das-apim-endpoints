using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;
public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
{
    private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;

    public GetAddressesQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient)
    {
        _locationApiClient = locationApiClient;
    }

    public const double MinimumMatch = 0.1;
    public const double MaximumMatch = 1;
    public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";




    public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var isFullPostcode = Regex.IsMatch(request.Query, PostcodeRegex);
        request.MinMatch = isFullPostcode ? MaximumMatch : MinimumMatch;

        var addressesResponse = await _locationApiClient.Get<GetAddressesQueryResult>(request);

        //var addresses = addressesResponse.Addresses.Select(a => (AddressItem)a);

        //return new GetAddressesQueryResult { Addresses = addresses };

        return addressesResponse;
    }
}