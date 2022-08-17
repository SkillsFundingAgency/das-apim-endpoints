using System.Collections.Generic;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;

namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships
{
    public class GetEditDraftApprenticeshipDeliveryModelResponse
    {
        public string DeliveryModel { get; set; }
        public List<string> DeliveryModels { get; set; }
        public bool HasUnavailableDeliveryModel { get; set; }
        public string EmployerName { get; set; }

        public static implicit operator GetEditDraftApprenticeshipDeliveryModelResponse(GetEditDraftApprenticeshipDeliveryModelQueryResult source)
        {
            return new GetEditDraftApprenticeshipDeliveryModelResponse
            {
                DeliveryModel = source.DeliveryModel.ToString(),
                DeliveryModels = source.DeliveryModels,
                HasUnavailableDeliveryModel = source.HasUnavailableDeliveryModel,
                EmployerName = source.EmployerName
            };
        }
    }
}
