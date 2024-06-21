using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetCandidateApplicationsByVacancyRequest(string vacancyReference, Guid preferenceId) : IGetApiRequest
{
    public string GetUrl => $"api/Vacancies/{vacancyReference}/candidates?allowEmailContact=true&preferenceId={preferenceId}&applicationStatus=Draft";
}