using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;
using System.ComponentModel;
using System.Diagnostics;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

internal static class ApprenticeshipExtensions
{
    internal static Guid GetEpisodePriceKey(this Learning learning, short academicYear, byte deliveryPeriod)
    {
        var prices = learning.Episodes.SelectMany(e => e.Prices).ToList();
        var searchDateTime = academicYear.GetDateTime(deliveryPeriod).AddDays(14);
        var price = prices.FirstOrDefault(p => p.StartDate <= searchDateTime && p.EndDate >= searchDateTime);
        return price?.Key ?? Guid.Empty;
    }

    /// <summary>
    /// Sets the withdrawal date for an apprenticeship based on the specified withdrawal date type and returns the date that has been set
    /// </summary>
    internal static DateTime? SetWithdrawalDate(this Learning learning, WithdrawalDate withdrawalDate)
    {
        var duration = 1 + (learning.PlannedEndDate - learning.StartDate).Days;

        var qualifyingPeriod = 42;
        switch (duration)
        {
            case < 14: qualifyingPeriod =  1;
                break;
            case < 167: qualifyingPeriod = 14;
                break;
            default:
                break;
        }

        EpisodePrice? nextPriceEpisode = null;
        if (withdrawalDate == WithdrawalDate.AfterNextPriceEpisodeStart || withdrawalDate == WithdrawalDate.BeforeNextPriceEpisodeStart)
        {
            nextPriceEpisode = learning.Episodes
                .SelectMany(e => e.Prices)
                .OrderBy(p => p.StartDate)
                .ElementAtOrDefault(1);

            if (nextPriceEpisode == null)
                throw new InvalidOperationException($"No next price episode found, this is required when setting a withdrawl date for {withdrawalDate}");
        }

        switch (withdrawalDate)
        {
            case WithdrawalDate.None:
                break;
            case WithdrawalDate.DuringQualifyingPeriod:
                learning.WithdrawnDate = learning.StartDate.AddDays(qualifyingPeriod - 2);
                learning.Episodes.First().LastDayOfLearning = learning.WithdrawnDate;
                break;
            case WithdrawalDate.AfterQualifyingPeriod:
                learning.WithdrawnDate = learning.StartDate.AddDays(qualifyingPeriod - 1);
                learning.Episodes.First().LastDayOfLearning = learning.WithdrawnDate;
                break;
            case WithdrawalDate.BeforeNextPriceEpisodeStart:
                learning.WithdrawnDate = nextPriceEpisode!.StartDate.AddDays(-5);
                learning.Episodes.First().LastDayOfLearning = learning.WithdrawnDate;
                break;
            case WithdrawalDate.AfterNextPriceEpisodeStart:
                learning.WithdrawnDate = nextPriceEpisode!.StartDate.AddDays(5);
                learning.Episodes.First().LastDayOfLearning = learning.WithdrawnDate;
                break;
            default:
                throw new InvalidEnumArgumentException();
        }

        return learning.WithdrawnDate;
    }
}
