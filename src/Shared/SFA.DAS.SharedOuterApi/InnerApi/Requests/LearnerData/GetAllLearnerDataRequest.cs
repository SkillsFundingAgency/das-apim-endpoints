using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.LearnerData;

public class GetAllLearnerDataRequest(int page, int? pageSize, bool excludeApproved) : IGetApiRequest
{
    public string GetUrl => $"/learners?page={page}&pageSize={pageSize}&excludeApproved={excludeApproved.ToString().ToLower()}";
}
