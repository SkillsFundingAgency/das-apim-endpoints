using System.Collections.Generic;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;

namespace SFA.DAS.Approvals.Api.Models.Apprentices
{
    public class GetEditApprenticeshipDeliveryModelResponse
    {
        public string LegalEntityName { get; set; }
        public List<string> DeliveryModels { get; set; }

        public static implicit operator GetEditApprenticeshipDeliveryModelResponse(GetEditApprenticeshipDeliveryModelQueryResult source)
        {
            return new GetEditApprenticeshipDeliveryModelResponse
            {
                LegalEntityName = source.LegalEntityName,
                DeliveryModels = source.DeliveryModels
            };
        }
    }
}
