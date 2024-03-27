using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
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
        var result = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByFullPostcodeRequest(request.Postcode));


        if (result?.Postcode is not null)
        {
            return new GetCandidatePostcodeAddressQueryResult { PostcodeExists = true };
        }
        else
        {
            return new GetCandidatePostcodeAddressQueryResult { PostcodeExists = false };
        }
    }
}
