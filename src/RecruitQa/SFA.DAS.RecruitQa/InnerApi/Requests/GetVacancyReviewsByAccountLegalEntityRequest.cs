using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByAccountLegalEntityRequest(long accountLegalEntityId) : IGetApiRequest
{
    public string GetUrl => $"api/accounts/{accountLegalEntityId}/vacancyreviews";
}