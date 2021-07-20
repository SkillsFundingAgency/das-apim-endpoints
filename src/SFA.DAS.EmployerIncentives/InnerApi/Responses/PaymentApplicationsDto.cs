using System;
using SFA.DAS.EmployerIncentives.Models;
using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Responses
{
    public class PaymentApplicationsDto
    {
        public IEnumerable<ApprenticeApplication> ApprenticeApplications { get; set; }
        public BankDetailsStatus BankDetailsStatus { get; set; }
        public Guid FirstSubmittedApplicationId { get; set; }
    }
}
