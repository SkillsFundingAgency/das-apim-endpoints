using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.LearnerData.Application.Fm36.PriceEpisodeHelper;

internal static class PriceEpisodeBuilder
{
    internal static List<PriceEpisode> GetPriceEpisodes(
        JoinedLearnerData joinedLearnerData,
        GetAcademicYearsResponse currentAcademicYear,
        byte collectionPeriod)
    {
        var priceEpisodesForAcademicYear = GetPriceEpisodesByFm36StartAndFinishPeriods(joinedLearnerData.Episodes, currentAcademicYear)
            .ToList();

        //If there are no price episodes for the requested academic year, create one, using the values of the first actual episode
        if (priceEpisodesForAcademicYear.Count == 0)
        {
            priceEpisodesForAcademicYear =
            [
                new JoinedPriceEpisode(joinedLearnerData.Episodes.First(), currentAcademicYear.StartDate, currentAcademicYear.EndDate, currentAcademicYear.GetShortAcademicYear(), true)
            ];
        }


        var lastPriceEpisode = priceEpisodesForAcademicYear.MaxBy(x => x.EndDate);

        return priceEpisodesForAcademicYear
            .Select(priceEpisodeModel => {

                var hasSubsequentPriceEpisodes = lastPriceEpisode != null && priceEpisodeModel.EndDate != lastPriceEpisode.EndDate;
                return new PriceEpisode
                {
                    PriceEpisodeIdentifier = $"{EarningsFM36Constants.ProgType}-{priceEpisodeModel.TrainingCode.Trim()}-{priceEpisodeModel.StartDate:dd/MM/yyyy}",

                    PriceEpisodeValues = joinedLearnerData.GetPriceEpisodeValues(priceEpisodeModel, currentAcademicYear, collectionPeriod, hasSubsequentPriceEpisodes),
                    PriceEpisodePeriodisedValues = joinedLearnerData.GetPriceEpisodePeriodisedValues(priceEpisodeModel, currentAcademicYear)
                };

            }).ToList();
    }

    // The fm36 expects PriceEpisodes to be split by academic year, so we need to split the price episodes by the academic year start and finish periods
    // the exception is where there is a price change within the academic year, in which case we need to split the price episodes by the start and finish periods of the price change
    private static IEnumerable<JoinedPriceEpisode> GetPriceEpisodesByFm36StartAndFinishPeriods(List<JoinedPriceEpisode> priceEpisodes, GetAcademicYearsResponse academicYearDetails)
    {
        foreach (var episodePrice in priceEpisodes)
        {
            if (episodePrice.StartDate <= academicYearDetails.StartDate && episodePrice.EndDate >= academicYearDetails.StartDate)
            {
                // Some of the later instalments are in the current academic year, capture these as a price episode
                yield return new JoinedPriceEpisode(episodePrice, academicYearDetails.StartDate, episodePrice.EndDate, academicYearDetails.GetShortAcademicYear(), false);
            }
            else if (episodePrice.StartDate < academicYearDetails.EndDate && episodePrice.EndDate > academicYearDetails.EndDate)
            {
                // Some of the earlier instalments are in the current academic year, capture these as a price episode
                yield return new JoinedPriceEpisode(episodePrice, episodePrice.StartDate, academicYearDetails.EndDate, academicYearDetails.GetShortAcademicYear(), true);
            }
            else if (episodePrice.StartDate > academicYearDetails.EndDate || episodePrice.EndDate < academicYearDetails.StartDate)
            {
                // no part of the price episode is in the current academic year, do not return it
            }
            else
            {
                // start and end dates are within the current academic year, so capture the price episode as is
                yield return episodePrice;
            }
        }
    }
}
