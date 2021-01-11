
using SFA.DAS.EmployerIncentives.Models;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class GetApplicationsResponse
    {
        public IEnumerable<ApprenticeApplication> ApprenticeApplications { get; set; }
        public BankDetailsStatus BankDetailsStatus { get; set; }
    }
}
