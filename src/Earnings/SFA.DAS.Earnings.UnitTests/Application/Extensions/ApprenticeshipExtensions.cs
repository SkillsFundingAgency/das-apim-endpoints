using System.ComponentModel;
using SFA.DAS.Earnings.Application.Extensions;
using SFA.DAS.Earnings.UnitTests.MockDataGenerator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.Earnings.UnitTests.Application.Extensions;

public static class ApprenticeshipExtensions
{
    public static Guid GetEpisodePriceKey(this Learning learning, short academicYear, byte deliveryPeriod)
    {
        var prices = learning.Episodes.SelectMany(e => e.Prices).ToList();
        var searchDateTime = academicYear.GetDateTime(deliveryPeriod).AddDays(14);
        var price = prices.FirstOrDefault(p => p.StartDate <= searchDateTime && p.EndDate >= searchDateTime);
        return price?.Key ?? Guid.Empty;
    }

    /// <summary>
    /// Sets the withdrawal date for an apprenticeship based on the specified withdrawal date type and returns the date that has been set
    /// </summary>
    public static DateTime? SetWithdrawalDate(this Learning learning, WithdrawalDate withdrawalDate)
    {
        var qualifyingPeriod = SharedOuterApi.Common.Constants.QualifyingPeriod;

        EpisodePrice? nextPriceEpisode = null;
        if(withdrawalDate == WithdrawalDate.AfterNextPriceEpisodeStart || withdrawalDate == WithdrawalDate.BeforeNextPriceEpisodeStart)
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
                learning.WithdrawnDate = learning.StartDate.AddDays(qualifyingPeriod - 1);
                learning.Episodes.First().LastDayOfLearning = learning.WithdrawnDate;
                break;
            case WithdrawalDate.AfterQualifyingPeriod:
                learning.WithdrawnDate = learning.StartDate.AddDays(qualifyingPeriod + 1);
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