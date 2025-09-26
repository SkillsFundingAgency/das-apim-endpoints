using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReview;

public class GetVacancyReviewQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) : IRequestHandler<GetVacancyReviewQuery, GetVacancyReviewQueryResult>
{
    public async Task<GetVacancyReviewQueryResult> Handle(GetVacancyReviewQuery request, CancellationToken cancellationToken)
    {
        var result =
            await recruitApiClient.GetWithResponseCode<GetVacancyReviewResponse>(
                new GetVacancyReviewByIdRequest(request.Id));

        if (result.StatusCode == HttpStatusCode.NotFound)
        {
            return new GetVacancyReviewQueryResult
            {
                VacancyReview = null
            };
        }

        return new GetVacancyReviewQueryResult
        {
            VacancyReview = result.Body
        };
    }
}