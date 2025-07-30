using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.ApplicationReview.Queries.GetPagedApplicationReviewsByVacancyReference;
public class GetPagedApplicationReviewsByVacancyReferenceQueryHandler(
    IRecruitApiClient<RecruitApiConfiguration> recruitApiClient)
    : IRequestHandler<GetPagedApplicationReviewsByVacancyReferenceQuery,
        GetPagedApplicationReviewsByVacancyReferenceQueryResult>
{
    public async Task<GetPagedApplicationReviewsByVacancyReferenceQueryResult> Handle(GetPagedApplicationReviewsByVacancyReferenceQuery request, CancellationToken cancellationToken)
    {
        var response = await recruitApiClient.Get<GetPagedApplicationReviewsByVacancyReferenceApiResponse>(
            new GetPagedApplicationReviewsByVacancyReferenceApiRequest(
                request.VacancyReference,
                request.PageNumber,
                request.PageSize,
                request.SortColumn,
                request.IsAscending));

        return response;
    }
}
