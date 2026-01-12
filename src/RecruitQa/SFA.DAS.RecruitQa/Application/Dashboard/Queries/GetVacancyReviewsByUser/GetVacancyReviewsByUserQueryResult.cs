using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Application.Dashboard.Queries.GetVacancyReviewsByUser;

public class GetVacancyReviewsByUserQueryResult
{
    public required List<GetVacancyReviewResponse> VacancyReviews { get; set; }
}
