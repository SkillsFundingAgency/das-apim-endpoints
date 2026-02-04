using SFA.DAS.LearnerDataJobs.Application.Commands;

namespace SFA.DAS.LearnerDataJobs.InnerApi;

public class LearnerDataRequest : LearnerDataIncomingRequest
{
    public string TrainingName { get; set; }
    public LearningType? ApprenticeshipType { get; set; }
}

public enum LearningType : byte
{
    Apprenticeship = 0,
    FoundationApprenticeship = 1,
    ApprenticeshipUnit = 2
}
