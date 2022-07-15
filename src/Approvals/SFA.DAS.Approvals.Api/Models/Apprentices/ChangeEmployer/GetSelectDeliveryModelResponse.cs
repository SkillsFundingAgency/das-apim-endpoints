using System.Collections.Generic;

namespace SFA.DAS.Approvals.Api.Models.Apprentices.ChangeEmployer
{
    public class GetSelectDeliveryModelResponse
    {
        public string LegalEntityName { get; set; }
        public List<string> DeliveryModels { get; set; }
    }
}
