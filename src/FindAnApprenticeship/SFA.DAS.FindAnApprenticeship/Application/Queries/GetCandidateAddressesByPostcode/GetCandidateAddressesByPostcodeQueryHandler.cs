using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Configuration;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
public class GetCandidateAddressesByPostcodeQueryHandler : IRequestHandler<GetCandidateAddressesByPostcodeQuery, GetCandidateAddressesByPostcodeQueryResult>
{
    private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
    private readonly FindAnApprenticeshipConfiguration _config;

    public GetCandidateAddressesByPostcodeQueryHandler(
        ILocationApiClient<LocationApiConfiguration> locationApiClient,
        FindAnApprenticeshipConfiguration config)
    {
        _locationApiClient = locationApiClient;
        _config = config;
    }

    public async Task<GetCandidateAddressesByPostcodeQueryResult> Handle(GetCandidateAddressesByPostcodeQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Postcode)) throw new ArgumentException($"Postcode is required", nameof(GetCandidateAddressesByPostcodeQuery.Postcode));

        var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(request.Postcode, _config.LocationsApiMinMatch));

        return new GetCandidateAddressesByPostcodeQueryResult(addressesResponse);
    }
}
