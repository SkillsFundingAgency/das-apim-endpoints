using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;

namespace SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;

public static class LearningDeliveryPeriodisedTextValuesBuilder
{
    public static LearningDeliveryPeriodisedTextValues BuildWithSameValues(string attributeName, string value)
    {
        return new LearningDeliveryPeriodisedTextValues
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
