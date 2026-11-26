using MediatR;
using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetPostcodeData;

public class GetPostcodeDataQueryHandler(ILocationLookupService locationLookupService) : IRequestHandler<GetPostcodeDataQuery, GetPostcodeDataResult>
{
    public async Task<GetPostcodeDataResult> Handle(GetPostcodeDataQuery request, CancellationToken cancellationToken)
    {
        ArgumentException.ThrowIfNullOrEmpty(request.Postcode);

        var postcodeInfo = await locationLookupService.GetPostcodeInfoAsync(request.Postcode);

        return postcodeInfo is not { Country: nameof(Country.England) } 
            ? GetPostcodeDataResult.None 
            : new GetPostcodeDataResult(postcodeInfo.Postcode, postcodeInfo.Country, postcodeInfo.Latitude, postcodeInfo.Longitude);
    }
}