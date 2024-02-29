﻿using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

namespace SFA.DAS.Apprenticeships.Api.Models
{
    public class GetPendingPriceChangeResponse
    {
	    public bool HasPendingPriceChange { get; set; }
	    public PendingPriceChange? PendingPriceChange { get; set; }
        public string ProviderName { get; set; }

		public GetPendingPriceChangeResponse(GetPendingPriceChangeApiResponse apiResponse, string providerName)
	    {
            HasPendingPriceChange = apiResponse.HasPendingPriceChange;
			PendingPriceChange = apiResponse.PendingPriceChange != null ? new PendingPriceChange
			{
				EffectiveFrom = apiResponse.PendingPriceChange.EffectiveFrom,
				OriginalAssessmentPrice = apiResponse.PendingPriceChange.OriginalAssessmentPrice,
				OriginalTotalPrice = apiResponse.PendingPriceChange.OriginalTotalPrice,
				OriginalTrainingPrice = apiResponse.PendingPriceChange.OriginalTrainingPrice,
				PendingAssessmentPrice = apiResponse.PendingPriceChange.PendingAssessmentPrice,
				PendingTotalPrice = apiResponse.PendingPriceChange.PendingTotalPrice,
				PendingTrainingPrice = apiResponse.PendingPriceChange.PendingTrainingPrice,
				Reason = apiResponse.PendingPriceChange.Reason,
				Ukprn = apiResponse.PendingPriceChange.Ukprn
			} : null;
            ProviderName = providerName;
        }
    }
}
