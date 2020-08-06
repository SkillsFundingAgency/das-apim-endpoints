using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public enum JobType
    {
        RefreshLegalEntities = 1
    }

    public class JobRequest
    {
        public JobType Type { get; set; }
        public Dictionary<string, object> Data { get; set; }
    }
}