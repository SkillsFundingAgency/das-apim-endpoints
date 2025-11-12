using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerDataJobs.InnerApi;

public class GetAllLearnersRequest(int page, int pageSize, bool excludeApproved) : IGetApiRequest
{
    public string GetUrl => $"learners?page={page}&pageSize={pageSize}&excludeApproved={excludeApproved.ToString().ToLower()}";
    public object Version { get; set; } = new { };
}
