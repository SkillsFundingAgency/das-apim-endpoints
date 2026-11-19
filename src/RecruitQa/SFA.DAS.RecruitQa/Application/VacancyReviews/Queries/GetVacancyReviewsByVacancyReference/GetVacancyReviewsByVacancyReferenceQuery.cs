using SFA.DAS.Recruit.Contracts.ApiResponses;
using SFA.DAS.SharedOuterApi.Types.Domain.Recruit.Reviews;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsByVacancyReference;

public class GetVacancyReviewsByVacancyReferenceQuery : IRequest<GetVacancyReviewsByVacancyReferenceQueryResult>
{
    public long VacancyReference { get; set; }
    public List<ReviewStatus>? Status { get; set; }
    public List<ManualQaOutcome>? ManualOutcome { get; set; }
    public bool IncludeNoStatus { get; set; }
}
