using SFA.DAS.Aodp.Application.Queries.Jobs;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobRunsApiResponse
    {
        public List<JobRun> JobRuns { get; set; } = new List<JobRun>();
    }   
}
