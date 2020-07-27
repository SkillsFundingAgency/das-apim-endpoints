using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Models.PassThrough
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