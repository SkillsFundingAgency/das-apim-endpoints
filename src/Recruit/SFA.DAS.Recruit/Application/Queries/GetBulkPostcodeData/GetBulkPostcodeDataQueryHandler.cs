using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetBulkPostcodeData;

public class GetBulkPostcodeDataQueryHandler(ILocationApiClient<LocationApiConfiguration> locationApiClient) : IRequestHandler<GetBulkPostcodeDataQuery, GetBulkPostcodeDataResult>
{
    public async Task<GetBulkPostcodeDataResult> Handle(GetBulkPostcodeDataQuery request, CancellationToken cancellationToken)
    {
        var result = await locationApiClient.PostWithResponseCode<GetBulkPostcodeDataResponse>(new GetLocationsByPostBulkPostcodeRequest(request.Postcodes));

        if (result.StatusCode != HttpStatusCode.OK)
        {
            return null;
        }

        var foundPostcodes = result.Body.Results ?? [];
        var results = request.Postcodes.Select(x =>
        {
            var match = foundPostcodes.FirstOrDefault(pdr => pdr.Postcode.Replace(" ", string.Empty).ToLowerInvariant().Equals(x.Replace(" ", string.Empty).ToLowerInvariant()));
            return new GetBulkPostcodeDataItemResult(x, match?.ToDomain());
        }).ToList();
        
        return new GetBulkPostcodeDataResult(results);
    }
}