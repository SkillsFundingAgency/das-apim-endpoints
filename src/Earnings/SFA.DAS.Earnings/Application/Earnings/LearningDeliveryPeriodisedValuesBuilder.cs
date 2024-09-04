using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;

namespace SFA.DAS.Earnings.Application.Earnings;

public static class LearningDeliveryPeriodisedValuesBuilder
{
    public static LearningDeliveryPeriodisedValues BuildWithSameValues(string attributeName, decimal value)
    {
        return new LearningDeliveryPeriodisedValues
        {
            AttributeName = attributeName,
            Period1 = value,
            Period2 = value,
            Period3 = value,
            Period4 = value,
            Period5 = value,
            Period6 = value,
            Period7 = value,
            Period8 = value,
            Period9 = value,
            Period10 = value,
            Period11 = value,
            Period12 = value
        };
    }
}