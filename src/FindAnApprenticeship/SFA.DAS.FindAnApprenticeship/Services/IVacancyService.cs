using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Services;

public interface IVacancyService
{
    public Task<IVacancy> GetVacancy(string vacancyReference);
    public Task<IVacancy> GetClosedVacancy(string vacancyReference);
    public Task<List<IVacancy>> GetVacancies(List<string> vacancyReferences);
    public string GetVacancyWorkLocation(IVacancy vacancy, bool cityNamesOnly = false);
}