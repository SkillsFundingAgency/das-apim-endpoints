using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;

public class GetCandidatePostcodeAddressQueryHandler : IRequestHandler<GetCandidatePostcodeAddressQuery, GetCandidatePostcodeAddressQueryResult>
{
    private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;

    public GetCandidatePostcodeAddressQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient)
    {
        _locationApiClient = locationApiClient;
    }

    public async Task<GetCandidatePostcodeAddressQueryResult> Handle(GetCandidatePostcodeAddressQuery request, CancellationToken cancellationToken)
    {
        var result = await _locationApiClient.Get<GetLocationByFullPostcodeRequestV2Response>(new GetLocationByFullPostcodeRequestV2(request.Postcode));
        return new GetCandidatePostcodeAddressQueryResult
        {
            PostcodeExists = result is { Postcode: not null }
        };
    }
}
