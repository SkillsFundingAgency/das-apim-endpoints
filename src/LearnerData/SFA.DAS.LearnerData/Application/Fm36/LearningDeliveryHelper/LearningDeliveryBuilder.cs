using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;

internal static class LearningDeliveryBuilder
{
    internal static List<LearningDelivery> GetLearningDeliveries(GetAcademicYearsResponse currentAcademicYear, JoinedEarningsApprenticeship joinedApprenticeship)
    {
        return
        [
            new LearningDelivery
            {
                AimSeqNumber = 1,
                LearningDeliveryValues = joinedApprenticeship.GetLearningDelivery(currentAcademicYear),
                LearningDeliveryPeriodisedValues = joinedApprenticeship.GetLearningDeliveryPeriodisedValues(currentAcademicYear),
                LearningDeliveryPeriodisedTextValues = joinedApprenticeship.GetLearningDeliveryPeriodisedTextValues()
            }
        ];
    }
}
