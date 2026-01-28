using SFA.DAS.FindApprenticeshipJobs.Application.Shared;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries;

public record GetLiveVacancyQueryResult
{
    public LiveVacancy? LiveVacancy { get; set; }
}