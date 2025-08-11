using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetProvidersByLarsCode;

public class GetProvidersByLarsCodeQueryHandler(
    IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
    : IRequestHandler<GetProvidersByLarsCodeQuery, GetProvidersByLarsCodeQueryResult>
{
    public async Task<GetProvidersByLarsCodeQueryResult> Handle(GetProvidersByLarsCodeQuery query,
        CancellationToken cancellationToken)
    {
        var page = 1;
        var courseDetails = await roatpCourseManagementApiClient.Get<GetCourseProvidersApiResponse>(new GetCourseProvidersByLarsCodeRequest(query.LarsCode, page));
        var providers = new List<ProviderData>(courseDetails.Providers);
        while (page++ < courseDetails.TotalPages)
        {
            courseDetails = await roatpCourseManagementApiClient.Get<GetCourseProvidersApiResponse>(new GetCourseProvidersByLarsCodeRequest(query.LarsCode, page));
            providers.AddRange(courseDetails.Providers);
        }

        var results = providers
            .Select(x => new ProviderByLarsCodeItem(x.ProviderName, x.Ukprn))
            .OrderBy(x => x.Name);
        return new GetProvidersByLarsCodeQueryResult(results);
    }
}