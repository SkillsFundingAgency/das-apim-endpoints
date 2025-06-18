using System.ComponentModel;
using SFA.DAS.Earnings.Application.Extensions;
using SFA.DAS.Earnings.UnitTests.MockDataGenerator;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;

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

    public static void SetWithdrawalDate(this Learning learning, WithdrawalDate withdrawalDate)
    {
        var qualifyingPeriod = SharedOuterApi.Common.Constants.QualifyingPeriod;

        switch (withdrawalDate)
        {
            case WithdrawalDate.None:
                break;
            case WithdrawalDate.DuringQualifyingPeriod:
                learning.WithdrawnDate = learning.StartDate.AddDays(qualifyingPeriod - 1);
                break;
            case WithdrawalDate.AfterQualifyingPeriod:
                learning.WithdrawnDate = learning.StartDate.AddDays(qualifyingPeriod + 1);
                break;
            default:
                throw new InvalidEnumArgumentException();
        }
    }
}