using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewSummaryRequest : IGetApiRequest
{
    public string GetUrl => "api/VacancyReviews/summary";
}
