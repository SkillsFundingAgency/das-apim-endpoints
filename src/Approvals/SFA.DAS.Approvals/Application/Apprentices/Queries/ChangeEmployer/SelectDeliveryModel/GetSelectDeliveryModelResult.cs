using System.Collections.Generic;
using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.SelectDeliveryModel
{
    public class GetSelectDeliveryModelResult
    {
        public long ApprenticeshipId { get; set; }
        public string LegalEntityName { get; set; }
        public List<string> DeliveryModels { get; set; }
        public ApprenticeshipStatus Status { get; set; }
    }
}