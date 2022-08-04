using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship;

namespace SFA.DAS.Approvals.Api.Models.Apprentices
{
    public class GetEditApprenticeshipResponse
    {
        public string ProviderName { get; set; }
        public string LegalEntityName { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }

        public static implicit operator GetEditApprenticeshipResponse(GetEditApprenticeshipQueryResult source)
        {
            return new GetEditApprenticeshipResponse
            {
                ProviderName = source.ProviderName,
                LegalEntityName = source.LegalEntityName,
                HasMultipleDeliveryModelOptions = source.HasMultipleDeliveryModelOptions
            };
        }
    }
}
