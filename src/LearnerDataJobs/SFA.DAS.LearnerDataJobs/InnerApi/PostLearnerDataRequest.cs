using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.InnerApi;

public class PostLearnerDataRequest(LearnerDataRequest data) : IPostApiRequest
{
    public string PostUrl => $"api/learners";
    public object Data { get; set; } = data;
}
