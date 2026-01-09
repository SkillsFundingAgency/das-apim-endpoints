using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByAccountLegalEntityRequest(long accountLegalEntityId) : IGetApiRequest
{
    public string GetUrl => $"accounts/{accountLegalEntityId}/vacancyreviews";
}