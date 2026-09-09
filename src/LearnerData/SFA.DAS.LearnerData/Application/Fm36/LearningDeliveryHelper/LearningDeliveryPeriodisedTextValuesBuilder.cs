using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Application.Fm36.Common;

namespace SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;

public static class LearningDeliveryPeriodisedTextValuesBuilder
{
    public static LearningDeliveryPeriodisedTextValues BuildWithSameValuesWhereActive(
        JoinedLearningDelivery learningDelivery, 
        string attributeName, 
        string value, 
        short academicYear, 
        InstalmentType instalmentType = InstalmentType.Regular)
    {
        var instalments = learningDelivery.GetInstalmentsForAcademicYear(academicYear, instalmentType);

        return new LearningDeliveryPeriodisedTextValues
        {
            AttributeName = attributeName,
            Period1 = instalments.Any(x=>x.DeliveryPeriod == 1) ? value : "None",
            Period2 = instalments.Any(x=>x.DeliveryPeriod == 2) ? value : "None",
            Period3 = instalments.Any(x=>x.DeliveryPeriod == 3) ? value : "None",
            Period4 = instalments.Any(x=>x.DeliveryPeriod == 4) ? value : "None",
            Period5 = instalments.Any(x=>x.DeliveryPeriod == 5) ? value : "None",
            Period6 = instalments.Any(x=>x.DeliveryPeriod == 6) ? value : "None",
            Period7 = instalments.Any(x=>x.DeliveryPeriod == 7) ? value : "None",
            Period8 = instalments.Any(x=>x.DeliveryPeriod == 8) ? value : "None",
            Period9 = instalments.Any(x => x.DeliveryPeriod == 9) ? value : "None",
            Period10 = instalments.Any(x=>x.DeliveryPeriod == 10) ? value : "None",
            Period11 = instalments.Any(x=>x.DeliveryPeriod == 11) ? value : "None",
            Period12 = instalments.Any(x => x.DeliveryPeriod == 12) ? value : "None"
        };
    }
}
