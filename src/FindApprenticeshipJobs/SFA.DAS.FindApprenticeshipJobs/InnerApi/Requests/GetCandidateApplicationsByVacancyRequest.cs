using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

public class GetCandidateApplicationsByVacancyRequest(string vacancyReference, Guid? preferenceId = null, bool filterEmailContactOnly = true) : IGetApiRequest
{
    public string GetUrl => $"api/Vacancies/{vacancyReference}/candidates?allowEmailContact={filterEmailContactOnly}&preferenceId={preferenceId}&applicationStatus=Draft";
}