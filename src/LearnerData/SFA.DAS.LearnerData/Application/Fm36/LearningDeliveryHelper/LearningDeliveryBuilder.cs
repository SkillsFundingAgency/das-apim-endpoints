using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Application.Fm36.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;

namespace SFA.DAS.LearnerData.Application.Fm36.LearningDeliveryHelper;

internal static class LearningDeliveryBuilder
{
    internal static List<LearningDelivery> GetLearningDeliveries(GetAcademicYearsResponse currentAcademicYear, JoinedLearnerData joinedLearnerData)
    {
        return joinedLearnerData.LearningDeliveries.Select(ld =>
            new LearningDelivery
            {
                AimSeqNumber = ld.AimSequenceNumber,
                LearningDeliveryValues = joinedLearnerData.GetLearningDelivery(ld, currentAcademicYear),
                LearningDeliveryPeriodisedValues = ld.GetLearningDeliveryPeriodisedValues(currentAcademicYear),
                LearningDeliveryPeriodisedTextValues = joinedLearnerData.GetLearningDeliveryPeriodisedTextValues()
            }
        ).ToList();

    }
}
