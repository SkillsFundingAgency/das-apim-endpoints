using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Abstract;

namespace SFA.DAS.Earnings.UnitTests.Application.Earnings;

public static class PeriodisedAttributeTestHelperExtensions
{
    public static bool AllValuesAreSetToZero(this PeriodisedAttribute attribute)
    {
        return AllValuesAreSetTo(attribute, 0);
    }

    public static bool AllValuesAreSetTo(this PeriodisedAttribute attribute, decimal? value)
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