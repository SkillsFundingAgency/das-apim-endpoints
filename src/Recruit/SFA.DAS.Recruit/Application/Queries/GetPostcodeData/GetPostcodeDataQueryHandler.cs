using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

public class GetPostcodeDataQueryHandler(ILocationLookupService locationLookupService) : IRequestHandler<GetPostcodeDataQuery, GetPostcodeDataResult>
{
    public async Task<GetPostcodeDataResult> Handle(GetPostcodeDataQuery request, CancellationToken cancellationToken)
    {
        var postcodeInfo = await locationLookupService.GetPostcodeInfoAsync(request.Postcode);
        return postcodeInfo is null
            ? GetPostcodeDataResult.None
            : new GetPostcodeDataResult(postcodeInfo.Postcode, postcodeInfo.Country, postcodeInfo.Latitude, postcodeInfo.Longitude);
    }
}