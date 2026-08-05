using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public record GetVacancyByIdRequest(Guid Id) : IGetApiRequest
{
    public string GetUrl => $"api/vacancies/{Id}";
}