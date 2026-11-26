using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public sealed record GetApplicationsByVacancyReferenceApiRequest(long VacancyReference)
        : IGetApiRequest
    {
        public string GetUrl => $"api/vacancies/{VacancyReference}/applications";
    }
}
