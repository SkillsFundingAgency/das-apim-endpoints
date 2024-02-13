﻿using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using static System.Text.RegularExpressions.Regex;

namespace SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;

public class GetAddressesQueryHandler : IRequestHandler<GetAddressesQuery, GetAddressesQueryResult>
{
    private readonly ILocationApiClient _locationApiClient;
    public const double MinimumMatch = 0.1;
    public const double MaximumMatch = 1;
    public const string PostcodeRegex = @"^[a-zA-z]{1,2}\d[a-zA-z\d]?\s*\d[a-zA-Z]{2}$";


    public GetAddressesQueryHandler(ILocationApiClient locationApiClient) => _locationApiClient = locationApiClient;

    public async Task<GetAddressesQueryResult> Handle(GetAddressesQuery request, CancellationToken cancellationToken)
    {
        var isFullPostcode = IsMatch(request.Query, PostcodeRegex);
        var minMatch = isFullPostcode ? MaximumMatch : MinimumMatch;

        var addressesResponse = await _locationApiClient.GetAddresses(request.Query, minMatch);

        var addresses = addressesResponse.Addresses.Select(a => (AddressItem)a);

        return new GetAddressesQueryResult { Addresses = addresses };
    }
}