using MediatR;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;

public class GetCandidatePostcodeAddressQueryHandler(ILocationLookupService locationLookupService)
    : IRequestHandler<GetCandidatePostcodeAddressQuery, GetCandidatePostcodeAddressQueryResult>
{
    public async Task<GetCandidatePostcodeAddressQueryResult> Handle(
        GetCandidatePostcodeAddressQuery request,
        CancellationToken cancellationToken)
    {
        return new GetCandidatePostcodeAddressQueryResult
        {
            PostcodeExists = await PostcodeExists(request.Postcode)
        };
    }

    private async Task<bool> PostcodeExists(string postcode)
    {
        var postcodeInfo = await locationLookupService.GetPostcodeInfoAsync(postcode);
        
        return postcodeInfo is not null;
    }
}