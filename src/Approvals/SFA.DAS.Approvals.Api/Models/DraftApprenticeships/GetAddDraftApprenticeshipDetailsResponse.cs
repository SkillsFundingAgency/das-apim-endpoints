﻿using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipDetails;

namespace SFA.DAS.Approvals.Api.Models.DraftApprenticeships
{
    public class GetAddDraftApprenticeshipDetailsResponse
    {
        public long AccountLegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
        public string StandardPageUrl { get; set; }
        public int? ProposedMaxFunding { get; set; }

        public static implicit operator GetAddDraftApprenticeshipDetailsResponse(GetAddDraftApprenticeshipDetailsQueryResult source)
        {
            return new GetAddDraftApprenticeshipDetailsResponse
            {
                AccountLegalEntityId = source.AccountLegalEntityId,
                LegalEntityName = source.LegalEntityName,
                ProviderName = source.ProviderName,
                HasMultipleDeliveryModelOptions = source.HasMultipleDeliveryModelOptions,
                StandardPageUrl = source.StandardPageUrl,
                ProposedMaxFunding = source.ProposedMaxFunding
            };
        }
    }
}
