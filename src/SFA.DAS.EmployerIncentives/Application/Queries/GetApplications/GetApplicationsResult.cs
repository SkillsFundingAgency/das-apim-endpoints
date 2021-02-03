using System;
using SFA.DAS.EmployerIncentives.InnerApi;
using SFA.DAS.EmployerIncentives.Models;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplications
{
    public class GetApplicationsResult
    {
        public IEnumerable<ApprenticeApplication> ApprenticeApplications { get; set; }
        public BankDetailsStatus BankDetailsStatus { get; set; }
        public Guid? FirstSubmittedApplicationId { get; set; }
    }
}
