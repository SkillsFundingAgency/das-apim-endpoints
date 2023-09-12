using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using System.Collections.Generic;
using System;
using System.Linq;

namespace SFA.DAS.Approvals.Extensions
{
    public static class PriceEpisodeExtensions
    {
        public static decimal GetPrice(this IEnumerable<GetPriceEpisodesResponse.PriceEpisode> priceEpisodes)
        {
            return priceEpisodes.GetPrice(DateTime.UtcNow);
        }

        public static decimal GetPrice(this IEnumerable<GetPriceEpisodesResponse.PriceEpisode> priceEpisodes,
            DateTime effectiveDate)
        {
            var episodes = priceEpisodes.ToList();

            var episode = episodes.FirstOrDefault(x =>
                x.FromDate <= effectiveDate && (x.ToDate == null || x.ToDate >= effectiveDate));

            return episode?.Cost ?? episodes.First().Cost;
        }
    }
}
