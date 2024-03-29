﻿using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Models;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;

namespace SFA.DAS.RoatpProviderModeration.Application.Provider.Queries.GetProvider
{
    public class GetProviderQueryResult
    {
        public int Ukprn { get; set; }
        public string LegalName { get; set; }
        public string MarketingInfo { get; set; }
        public ProviderType ProviderType { get; set; }
        public ProviderStatusType ProviderStatusType { get; set; }
        public DateTime? ProviderStatusUpdatedDate { get; set; }
        public bool IsProviderHasStandard { get; set; } = false;

        public static implicit operator GetProviderQueryResult(GetProviderResponse source) =>
            new GetProviderQueryResult
            {
                Ukprn = source.Ukprn,
                LegalName = source.LegalName,
                MarketingInfo = source.MarketingInfo,
                ProviderType = source.ProviderType,
                ProviderStatusType = source.ProviderStatusType,
                ProviderStatusUpdatedDate = source.ProviderStatusUpdatedDate,
                IsProviderHasStandard = source.IsProviderHasStandard
            };
    }
}