using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public enum JobType
    {
        RefreshLegalEntities = 1,
        RefreshEmploymentChecks = 2
    }

    public class JobRequest
    {
        public JobType Type { get; set; }        
        public Dictionary<string, string> Data { get; set; }
    }
}