using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Extensions;

public static class PriceEpisodePeriodisedValuesExtensions
{
    public static decimal? GetPeriodValue(this PriceEpisodePeriodisedValues item, int period)
    {
        switch (period)
        {
            case 1: return item.Period1;
            case 2: return item.Period2;
            case 3: return item.Period3;
            case 4: return item.Period4;
            case 5: return item.Period5;
            case 6: return item.Period6;
            case 7: return item.Period7;
            case 8: return item.Period8;
            case 9: return item.Period9;
            case 10: return item.Period10;
            case 11: return item.Period11;
            case 12: return item.Period12;
            default:
                throw new ArgumentOutOfRangeException(nameof(period), "Period must be between 1 and 12");
        }
    }

    public static decimal? GetPeriodValue(this LearningDeliveryPeriodisedValues item, int period)
    {
        switch (period)
        {
            case 1: return item.Period1;
            case 2: return item.Period2;
            case 3: return item.Period3;
            case 4: return item.Period4;
            case 5: return item.Period5;
            case 6: return item.Period6;
            case 7: return item.Period7;
            case 8: return item.Period8;
            case 9: return item.Period9;
            case 10: return item.Period10;
            case 11: return item.Period11;
            case 12: return item.Period12;
            default:
                throw new ArgumentOutOfRangeException(nameof(period), "Period must be between 1 and 12");
        }
    }


}
