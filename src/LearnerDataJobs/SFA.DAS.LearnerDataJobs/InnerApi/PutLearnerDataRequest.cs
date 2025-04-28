using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.InnerApi;

public class PutLearnerDataRequest(long providerId, long uln, int academicYear, int standardCode, LearnerDataRequest data) : IPutApiRequest
{
    public string PutUrl => $"providers/{providerId}/learners/{uln}/academicyears/{academicYear}/standardcodes/{standardCode}";
    public object Data { get; set; } = data;
}
