using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

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

