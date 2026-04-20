using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;

namespace SFA.DAS.LearnerData.UnitTests.Application.Fm36.TestHelpers;

internal static class LearningDeliveryPeriodisedTextValuesTestHelperExtensions
{
    internal static bool AllValuesAreSetTo(this LearningDeliveryPeriodisedTextValues attribute, string? value)
    {
        return attribute.Period1 == value
               && attribute.Period2 == value
               && attribute.Period3 == value
               && attribute.Period4 == value
               && attribute.Period5 == value
               && attribute.Period6 == value
               && attribute.Period7 == value
               && attribute.Period8 == value
               && attribute.Period9 == value
               && attribute.Period10 == value
               && attribute.Period11 == value
               && attribute.Period12 == value;
    }
}
