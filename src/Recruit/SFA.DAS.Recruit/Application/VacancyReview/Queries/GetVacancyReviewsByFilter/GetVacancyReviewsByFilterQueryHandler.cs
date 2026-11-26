using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyReviews;
using SFA.DAS.Recruit.InnerApi.Recruit.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByFilter;

public class GetVacancyReviewsByFilterQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetVacancyReviewsByFilterQuery, GetVacancyReviewsByFilterQueryResult>
{
    public async Task<GetVacancyReviewsByFilterQueryResult> Handle(GetVacancyReviewsByFilterQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.GetWithResponseCode<List<GetVacancyReviewResponse>>(
            new GetVacancyReviewsByFilterRequest(request.Status, request.ExpiredAssignationDateTime));

        if (response.StatusCode == HttpStatusCode.NotFound || response.Body == null)
        {
            return new GetVacancyReviewsByFilterQueryResult
            {
                VacancyReviews = new List<GetVacancyReviewResponse>()
            };
        }

        return new GetVacancyReviewsByFilterQueryResult
        {
            VacancyReviews = response.Body
        };
    }
}