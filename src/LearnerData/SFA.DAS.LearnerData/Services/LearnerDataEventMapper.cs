using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Services;

public interface ILearnerDataEventMapper
{
    LearnerDataEvent Build(
        long ukprn,
        LearnerRequestDetails learner,
        OnProgrammeRequestDetails onProgramme,
        LearningType learningType,
        Guid correlationId,
        DateTime receivedDate,
        string? consumerReference);
}

public class LearnerDataEventMapper : ILearnerDataEventMapper
{
    public LearnerDataEvent Build(
        long ukprn,
        LearnerRequestDetails learner,
        OnProgrammeRequestDetails onProgramme,
        LearningType learningType,
        Guid correlationId,
        DateTime receivedDate,
        string? consumerReference)
    {
        var cost = onProgramme.Costs.GetCostsOrDefault(onProgramme.StartDate).First();

        return new LearnerDataEvent
        {
            ULN = learner.Uln,
            UKPRN = ukprn,
            FirstName = learner.FirstName,
            LastName = learner.LastName,
            Email = learner.Email,
            DoB = learner.Dob,
            StartDate = onProgramme.StartDate,
            PlannedEndDate = onProgramme.ExpectedEndDate,
            PercentageLearningToBeDelivered = onProgramme.PercentageOfTrainingLeft,
            EpaoPrice = cost.EpaoPrice ?? 0,
            TrainingPrice = cost.TrainingPrice,
            AgreementId = onProgramme.AgreementId,
            IsFlexiJob = onProgramme.IsFlexiJob!.Value,
            StandardCode = onProgramme.StandardCode,
            CorrelationId = correlationId,
            ReceivedDate = receivedDate,
            ConsumerReference = consumerReference,
            LearningType = learningType
        };
    }
}
