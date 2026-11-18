using SFA.DAS.Recruit.Contracts.ApiResponses;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;

public class GetVacancyReviewsCountByAccountLegalEntityQuery : IRequest<GetVacancyReviewsCountByAccountLegalEntityQueryResult>
{
    public long AccountLegalEntityId { get; set; }
    public List<ReviewStatus>? Status { get; set; }
    public List<string>? ManualOutcome { get; set; }
    public EmployerNameOption? EmployerNameOption { get; set; }
}

public class GetVacancyReviewsCountByAccountLegalEntityQueryResult
{
    public int Count { get; set; }
}