using SFA.DAS.FindApprenticeshipJobs.Application.Shared;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.CivilServiceJobs;
public record GetCivilServiceJobsQueryResult
{
    public List<LiveVacancy> CivilServiceVacancies { get; set; } = [];
}