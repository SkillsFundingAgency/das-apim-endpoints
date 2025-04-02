using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class GetJobRunsApiRequest : IGetApiRequest
    {
        public string? JobName { get; set; }     

        public string GetUrl
        {
            get
            {
                return $"api/job/{JobName}/runs";
            }
        }
    }
}
