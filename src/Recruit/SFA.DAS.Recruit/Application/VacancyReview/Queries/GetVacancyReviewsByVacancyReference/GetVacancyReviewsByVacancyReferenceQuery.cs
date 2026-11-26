using MediatR;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.Application.VacancyReview.Queries.GetVacancyReviewsByVacancyReference;

public class GetVacancyReviewsByVacancyReferenceQuery : IRequest<GetVacancyReviewsByVacancyReferenceQueryResult>
{
    public long VacancyReference { get; set; }
    public string? Status { get; set; }
    public List<string>? ManualOutcome { get; set; }
    public bool IncludeNoStatus { get; set; }
}