using System.Collections.Generic;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDeliveryModel;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class GetAddDraftApprenticeshipDeliveryModelResponse
    {
        public List<string> DeliveryModels { get; set; }
        public string EmployerName { get; set; }

        public static implicit operator GetAddDraftApprenticeshipDeliveryModelResponse(GetAddDraftApprenticeshipDeliveryModelQueryResult source)
        {
            return new GetAddDraftApprenticeshipDeliveryModelResponse
            {
                DeliveryModels = source.DeliveryModels,
                EmployerName = source.EmployerName
            };
        }
    }
}
