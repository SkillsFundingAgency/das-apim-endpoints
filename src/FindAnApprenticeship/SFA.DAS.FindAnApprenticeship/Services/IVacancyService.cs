using System.Collections.Generic;
using System.Threading.Tasks;
using static SFA.DAS.FindAnApprenticeship.InnerApi.Responses.PostGetVacanciesByReferenceApiResponse;

namespace SFA.DAS.FindAnApprenticeship.Services;

public interface IVacancyService
{
    public Task<IVacancy> GetVacancy(string vacancyReference);
    public Task<IVacancy> GetClosedVacancy(string vacancyReference);
    public Task<List<ApprenticeshipVacancy>> GetVacancies(List<string> vacancyReferences);
    public Task<List<IVacancy>> GetClosedVacancies(List<string> vacancyReferences);
}