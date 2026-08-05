using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewByIdRequest(Guid id) : IGetApiRequest
{
    public string GetUrl => $"api/vacancyreviews/{id}";
}