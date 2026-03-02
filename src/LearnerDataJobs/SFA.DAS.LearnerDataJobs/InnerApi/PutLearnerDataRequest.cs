using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.InnerApi;

public class PutLearnerDataRequest(long providerId, long uln, LearnerDataRequest data) : IPutApiRequest
{
    public string PutUrl => $"providers/{providerId}/learners/{uln}";
    public object Data { get; set; } = data;
}
