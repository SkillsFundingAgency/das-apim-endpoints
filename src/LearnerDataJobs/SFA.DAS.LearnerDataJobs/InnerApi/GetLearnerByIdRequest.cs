using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.LearnerDataJobs.InnerApi;
public class GetLearnerByIdRequest(long providerId, long learnerDataId) : IGetApiRequest
{
    public string GetUrl => $"providers/{providerId}/learners/{learnerDataId}";
}
