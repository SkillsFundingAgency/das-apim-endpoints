using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Standards.Queries.GetStandards;

public class GetStandardsQueryHandler(ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient) : IRequestHandler<GetStandardsQuery, GetStandardsQueryResult>
{
    public async Task<GetStandardsQueryResult> Handle(GetStandardsQuery request, CancellationToken cancellationToken)
    {
        var standardsResponse = await _coursesApiClient.GetWithResponseCode<GetStandardsListResponse>(new GetActiveStandardsListRequest());

        standardsResponse.EnsureSuccessStatusCode();

        var standards = standardsResponse.Body.Standards.Select(s => (Standard)s).ToList();

        return new GetStandardsQueryResult(standards);
    }
}

