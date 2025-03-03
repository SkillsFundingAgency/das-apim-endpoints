using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetVacancyPreview;

public class GetVacancyPreviewQueryHandler(ICoursesApiClient<CoursesApiConfiguration> coursesApiClient) : IRequestHandler<GetVacancyPreviewQuery, GetVacancyPreviewQueryResult>
{
    public async Task<GetVacancyPreviewQueryResult> Handle(GetVacancyPreviewQuery request, CancellationToken cancellationToken)
    {
        var apiResponse =
            await coursesApiClient.GetWithResponseCode<GetStandardsListItem>(
                new GetStandardRequest(request.StandardId));

        if (apiResponse.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyPreviewQueryResult
            {
                Course = null
            };
        }

        return new GetVacancyPreviewQueryResult
        {
            Course = apiResponse.Body
        };
    }
}