using System;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.ApprenticeshipDetails
{
    public class ApprenticeshipDetailsRequest
    {
        public long AccountId { get; set; }
        public Guid ApplicationId { get; set; }
        public List<ApprenticeDetailsDto> ApprenticeshipDetails { get; set; }
    }
}
