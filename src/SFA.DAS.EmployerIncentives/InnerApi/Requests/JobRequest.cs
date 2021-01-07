using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests
{
    public enum JobType
    {
        [Obsolete]
        RefreshLegalEntities = 1,
        UpdateVrfCaseDetailsForNewApplications = 2,
        UpdateVrfCaseStatusForIncompleteCases = 3
    }

    public class JobRequest
    {
        public JobType Type { get; set; }        
        public Dictionary<string, string> Data { get; set; }
    }
}