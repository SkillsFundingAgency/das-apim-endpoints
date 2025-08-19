using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.InnerApi;

public class PatchLearnerDataApprenticeshipIdRequest(long providerId, long learnerDataId, LearnerDataApprenticeshipIdRequest data) : IPatchApiRequest<LearnerDataApprenticeshipIdRequest>
{
    public string PatchUrl => $"providers/{providerId}/learners/{learnerDataId}/apprenticeshipId";
    public LearnerDataApprenticeshipIdRequest Data { get; set; } = data;
}
