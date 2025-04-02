using SFA.DAS.Recruit.Application.Queries.GetPostcodeData;
using SFA.DAS.SharedOuterApi.Domain;

namespace SFA.DAS.Recruit.Api.Models;

public record GetPostcodeDataResponse(string Query, PostcodeData Result)
{
    public static GetPostcodeDataResponse From(string query, GetPostcodeDataResult source)
    {
        if (source is null || source == GetPostcodeDataResult.None)
        {
            return new GetPostcodeDataResponse(query, null);
        }

        return new GetPostcodeDataResponse(query, new PostcodeData(source.Postcode, source.Country, source.Latitude, source.Longitude));
    }
};