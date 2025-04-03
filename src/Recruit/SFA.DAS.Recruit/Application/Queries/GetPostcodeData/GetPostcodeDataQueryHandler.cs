using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

public class GetPostcodeDataQueryHandler(ILocationLookupService locationLookupService) : IRequestHandler<GetPostcodeDataQuery, GetPostcodeDataResult>
{
    public async Task<GetPostcodeDataResult> Handle(GetPostcodeDataQuery request, CancellationToken cancellationToken)
    {
        var location = await locationLookupService.GetLocationInformation(request.Postcode, 0, 0);
        return location is null
            ? GetPostcodeDataResult.None
            : new GetPostcodeDataResult(location.Name, location.Country, location.GeoPoint[0], location.GeoPoint[1]);
    }
}