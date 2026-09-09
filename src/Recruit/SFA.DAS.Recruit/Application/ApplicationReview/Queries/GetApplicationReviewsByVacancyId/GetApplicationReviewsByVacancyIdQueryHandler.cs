using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetApplicationReviewsByVacancyId;

public sealed class GetApplicationReviewsByVacancyIdQueryHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient) 
    : IRequestHandler<GetApplicationReviewsByVacancyIdQuery, GetApplicationReviewsByVacancyIdQueryResult>
{
    public async Task<GetApplicationReviewsByVacancyIdQueryResult> Handle(GetApplicationReviewsByVacancyIdQuery request, CancellationToken cancellationToken)
    {
        var recruitApiResponse = await recruitApiClient.Get<List<Domain.ApplicationReview>>(
            new GetApplicationReviewsByVacancyIdApiRequest(request.VacancyId));

        if (recruitApiResponse != null)
        {
            return new GetApplicationReviewsByVacancyIdQueryResult(recruitApiResponse);
        }

        return new GetApplicationReviewsByVacancyIdQueryResult([]);
    }
}
