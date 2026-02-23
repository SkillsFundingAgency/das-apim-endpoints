using MediatR;

namespace SFA.DAS.RecruitQa.Application.VacancyReviews.Queries.GetVacancyReviewsCountByAccountLegalEntity;

public class GetVacancyReviewsCountByAccountLegalEntityQuery : IRequest<GetVacancyReviewsCountByAccountLegalEntityQueryResult>
{
    public long AccountLegalEntityId { get; set; }
    public string? Status { get; set; }
    public string? ManualOutcome { get; set; }
    public string? EmployerNameOption { get; set; }
}

public class GetVacancyReviewsCountByAccountLegalEntityQueryResult
{
    public int Count { get; set; }
}
