using System.Collections.Generic;
using System;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses
{
    public class GetPriceEpisodesResponse
    {
        public class PriceEpisode
        {
            public long Id { get; set; }
            public long ApprenticeshipId { get; set; }
            public decimal Cost { get; set; }
            public decimal? TrainingPrice { get; set; }
            public decimal? EndPointAssessmentPrice { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime? ToDate { get; set; }
        }
        public IReadOnlyCollection<PriceEpisode> PriceEpisodes { get; set; }
    }
}
