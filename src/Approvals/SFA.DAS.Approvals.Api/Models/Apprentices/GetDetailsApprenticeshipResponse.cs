using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.DetailsApprenticeship;

namespace SFA.DAS.Approvals.Api.Models.Apprentices
{
    public class GetDetailsApprenticeshipResponse
    {
        public bool HasMultipleDeliveryModelOptions { get; set; }

        public static implicit operator GetDetailsApprenticeshipResponse(GetDetailsApprenticeshipQueryResult source)
        {
            return new GetDetailsApprenticeshipResponse
            {
                HasMultipleDeliveryModelOptions = source.HasMultipleDeliveryModelOptions
            };
        }
    }
}
