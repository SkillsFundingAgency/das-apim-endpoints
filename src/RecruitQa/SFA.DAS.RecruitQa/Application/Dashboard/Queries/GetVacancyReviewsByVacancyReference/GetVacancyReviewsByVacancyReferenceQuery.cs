using MediatR;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByVacancyReference;

public class GetVacancyReviewsByVacancyReferenceQuery : IRequest<GetVacancyReviewsByVacancyReferenceQueryResult>
{
    public long VacancyReference { get; set; }
    public string? Status { get; set; }
    public List<string>? ManualOutcome { get; set; }
    public bool IncludeNoStatus { get; set; }
}
