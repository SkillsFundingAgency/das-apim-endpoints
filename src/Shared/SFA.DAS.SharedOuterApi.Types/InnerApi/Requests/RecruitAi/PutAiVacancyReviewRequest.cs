using SFA.DAS.SharedOuterApi.Types.Domain.Domain.Recruit.Ai;

using SFA.DAS.Apim.Shared.Interfaces;

using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.RecruitAi;

public class PutAiVacancyReviewRequest(Guid vacancyReviewId, PutAiVacancyReviewDto data) : IPutApiRequest<PutAiVacancyReviewDto>
{
    public string PutUrl => $"api/ai-vacancy-reviews/{vacancyReviewId}";
    public PutAiVacancyReviewDto Data { get; set; } = data;
}

public class PutAiVacancyReviewDto
{
    public bool ManualReviewRequired { get; set; }
    public string? Output { get; set; }
    public AiReviewStatus? Status { get; set; }
    public Guid? VacancyId { get; set; }
}