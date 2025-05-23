using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Extensions;

public static class PriceEpisodePeriodisedValuesExtensions
{
    public static decimal? GetPeriodValue(this PriceEpisodePeriodisedValues item, int period)
    {
        return period switch
        {
            1 => item.Period1,
            2 => item.Period2,
            3 => item.Period3,
            4 => item.Period4,
            5 => item.Period5,
            6 => item.Period6,
            7 => item.Period7,
            8 => item.Period8,
            9 => item.Period9,
            10 => item.Period10,
            11 => item.Period11,
            12 => item.Period12,
            _ => throw new ArgumentOutOfRangeException(nameof(period), "Period must be between 1 and 12")
        };
    }

    public static decimal? GetPeriodValue(this LearningDeliveryPeriodisedValues item, int period)
    {
        return period switch
        {
            1 => item.Period1,
            2 => item.Period2,
            3 => item.Period3,
            4 => item.Period4,
            5 => item.Period5,
            6 => item.Period6,
            7 => item.Period7,
            8 => item.Period8,
            9 => item.Period9,
            10 => item.Period10,
            11 => item.Period11,
            12 => item.Period12,
            _ => throw new ArgumentOutOfRangeException(nameof(period), "Period must be between 1 and 12")
        };
    }
}
