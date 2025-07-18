using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public sealed record GetApplicationsByVacancyReferenceApiRequest(long VacancyReference)
        : IGetApiRequest
    {
        public string GetUrl => $"api/vacancies/{VacancyReference}/applications";
    }
}
