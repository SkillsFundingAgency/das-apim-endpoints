using SFA.DAS.FindApprenticeshipJobs.Application.Shared;

namespace SFA.DAS.FindApprenticeshipJobs.Application.Queries.NhsJobs;

public class GetNhsJobsQueryResult
{
    public List<LiveVacancy> NhsVacancies { get; set; }
}