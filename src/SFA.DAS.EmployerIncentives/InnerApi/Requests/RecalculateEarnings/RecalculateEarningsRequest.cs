using System.Collections.Generic;

namespace SFA.DAS.EmployerIncentives.InnerApi.Requests.RecalculateEarnings
{
    public class RecalculateEarningsRequest
    {
        public List<IncentiveLearnerIdentifierDto> IncentiveLearnerIdentifiers { get; set; }
    }
}
