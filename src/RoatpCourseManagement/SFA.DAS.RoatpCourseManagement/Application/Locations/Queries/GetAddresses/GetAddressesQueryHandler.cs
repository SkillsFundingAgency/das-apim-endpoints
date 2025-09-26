using MediatR;
using SFA.DAS.RoatpCourseManagement.Application.AddressLookup.Queries;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Queries.GetAddresses;
public class GetAddressesQueryHandler(ILocationApiClient<LocationApiConfiguration> _locationApiClient) : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
{
    public const double MinimumMatch = 0.1;
    public const double MaximumMatch = 1;
    public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";
    private readonly TimeSpan _regexTimeOut = TimeSpan.FromMilliseconds(500);

    public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var isFullPostcode = Regex.IsMatch(request.Query, PostcodeRegex, RegexOptions.None, _regexTimeOut);
        request.MinMatch = isFullPostcode ? MaximumMatch : MinimumMatch;

        var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(request);

        var addresses = addressesResponse.Addresses.Select(a => (AddressItem)a);

        return new GetAddressesQueryResult { Addresses = addresses };
    }
}