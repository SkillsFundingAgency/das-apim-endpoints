using SFA.DAS.Common.Domain.Types;
using SFA.DAS.LearnerData.Events;
using SFA.DAS.LearnerData.Extensions;
using SFA.DAS.LearnerData.Requests;

namespace SFA.DAS.LearnerData.Services;

public interface ILearnerDataEventMapper
{
    LearnerDataEvent Build(
        long ukprn,
        long uln,
        string firstName,
        string lastName,
        string? email,
        DateTime dob,
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
        long uln,
        string firstName,
        string lastName,
        string? email,
        DateTime dob,
        OnProgrammeRequestDetails onProgramme,
        LearningType learningType,
        Guid correlationId,
        DateTime receivedDate,
        string? consumerReference)
    {
        var cost = onProgramme.Costs.GetCostsOrDefault(onProgramme.StartDate).First();

        return new LearnerDataEvent
        {
            ULN = uln,
            UKPRN = ukprn,
            FirstName = firstName,
            LastName = lastName,
            Email = email,
            DoB = dob,
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
