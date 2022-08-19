using SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDetails;

namespace SFA.DAS.Approvals.Api.Models.Cohorts
{
    public class GetAddDraftApprenticeshipDetailsResponse
    {
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }

        public static implicit operator GetAddDraftApprenticeshipDetailsResponse(GetAddDraftApprenticeshipDetailsQueryResult source)
        {
            return new GetAddDraftApprenticeshipDetailsResponse
            {
                LegalEntityName = source.LegalEntityName,
                ProviderName = source.ProviderName,
                HasMultipleDeliveryModelOptions = source.HasMultipleDeliveryModelOptions
            };
        }
    }
}
