using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using Apprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Apprenticeship;
using Episode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships.Episode;
using EarningsApprenticeship = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Apprenticeship;
using EarningsEpisode = SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings.Episode;

namespace SFA.DAS.Earnings.Application.Earnings
{
    // The Models in this file are used to join data from the Apprenticeships and Earnings APIs


    internal class JoinedEarningsApprenticeship
    {
        /// <summary>
        /// Returned from apprenticeships api
        /// </summary>
        internal Apprenticeship Apprenticeship { get; set; }

        /// <summary>
        /// Returned from earnings api
        /// </summary>
        internal EarningsApprenticeship EarningsApprenticeship { get; set; }

        internal JoinedEarningsApprenticeship(Apprenticeship apprenticeship, EarningsApprenticeship earningsApprenticeship)
        {
            Apprenticeship = apprenticeship;
            EarningsApprenticeship = earningsApprenticeship;
        }
    }

    internal class JoinedPriceEpisodeModel
    {
        /// <summary>
        /// Returned from apprenticeships api
        /// </summary>
        internal Episode ApprenticeshipEpisode { get; set; }

        /// <summary>
        /// Returned from apprenticeships api
        /// </summary>
        internal EpisodePrice ApprenticeshipEpisodePrice { get; set; }

        /// <summary>
        /// Returned from earnings api
        /// </summary>
        internal EarningsEpisode EarningsEpisode { get; set; }

        internal JoinedPriceEpisodeModel(Episode episode, EpisodePrice price, EarningsEpisode earningsEpisode)
        {
            ApprenticeshipEpisode = episode;
            ApprenticeshipEpisodePrice = price;
            EarningsEpisode = earningsEpisode;
        }
    }
}
