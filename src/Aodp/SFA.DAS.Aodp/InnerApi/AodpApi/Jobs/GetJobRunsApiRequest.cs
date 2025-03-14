using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobRunsApiRequest : IGetApiRequest
    {
        public string GetUrl => $"api/job-runs/";
    }
}
