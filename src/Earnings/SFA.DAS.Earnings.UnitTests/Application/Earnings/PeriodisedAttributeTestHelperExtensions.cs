using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Abstract;
using Newtonsoft.Json.Linq;

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

    public static bool HasExactPeriodValues(this PeriodisedAttribute? attribute, decimal[] values)
    {
        return attribute.Period1 == values[0]
            && attribute.Period2 == values[1]
            && attribute.Period3 == values[2]
            && attribute.Period4 == values[3]
            && attribute.Period5 == values[4]
            && attribute.Period6 == values[5]
            && attribute.Period7 == values[6]
            && attribute.Period8 == values[7]
            && attribute.Period9 == values[8]
            && attribute.Period10 == values[9]
            && attribute.Period11 == values[10]
            && attribute.Period12 == values[11];
    }
}