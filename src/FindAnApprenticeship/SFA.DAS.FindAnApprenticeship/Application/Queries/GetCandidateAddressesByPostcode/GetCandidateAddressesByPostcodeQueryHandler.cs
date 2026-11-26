using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Configuration;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddressesByPostcode;
public class GetCandidateAddressesByPostcodeQueryHandler : IRequestHandler<GetCandidateAddressesByPostcodeQuery, GetCandidateAddressesByPostcodeQueryResult>
{
    private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;
    private readonly FindAnApprenticeshipConfiguration _config;

    public GetCandidateAddressesByPostcodeQueryHandler(
        ILocationApiClient<LocationApiConfiguration> locationApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        FindAnApprenticeshipConfiguration config)
    {
        _locationApiClient = locationApiClient;
        _candidateApiClient = candidateApiClient;
        _config = config;
    }

    public async Task<GetCandidateAddressesByPostcodeQueryResult> Handle(GetCandidateAddressesByPostcodeQuery request, CancellationToken cancellationToken)
    {
        var candidateAddress = await _candidateApiClient.Get<GetCandidateAddressApiResponse>(new GetCandidateAddressApiRequest(request.CandidateId));

        var postcode = request.Postcode ?? candidateAddress.Postcode;

        if (string.IsNullOrEmpty(postcode))
        {
            throw new InvalidOperationException($"A postcode is required in order to perform an address lookup, but the existing candidate did not have one and no postcode was specified");
        }

        var addressesResponse = await _locationApiClient.Get<GetAddressesListResponse>(new GetAddressesQueryRequest(request.Postcode, _config.LocationsApiMinMatch));

        return new GetCandidateAddressesByPostcodeQueryResult(candidateAddress, addressesResponse, postcode);
    }
}
