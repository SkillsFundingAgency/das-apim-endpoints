using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewStatsByVacancyReference;
public class GetApplicationReviewStatsByVacancyReferenceQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) :
    IRequestHandler<GetApplicationReviewStatsByVacancyReferenceQuery, GetApplicationReviewStatsByVacancyReferenceQueryResult>
{
    public async Task<GetApplicationReviewStatsByVacancyReferenceQueryResult> Handle(GetApplicationReviewStatsByVacancyReferenceQuery request, CancellationToken cancellationToken)
    {
        var recruitApiResponse = await recruitApiClient.Get<GetApplicationReviewsCountApiResponse>(
            new GetApplicationReviewsCountByVacancyReferenceApiRequest(request.VacancyReference));

        return recruitApiResponse == null 
            ? new GetApplicationReviewStatsByVacancyReferenceQueryResult() 
            : GetApplicationReviewStatsByVacancyReferenceQueryResult.FromApplicationReviewsCountResponse(recruitApiResponse);
    }
}