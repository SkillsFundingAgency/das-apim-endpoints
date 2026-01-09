using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByAccountLegalEntity;

public class GetVacancyReviewsByAccountLegalEntityQueryResult
{
    public required List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}