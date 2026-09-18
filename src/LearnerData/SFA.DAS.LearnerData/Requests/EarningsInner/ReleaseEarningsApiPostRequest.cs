using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class ReleaseEarningsApiPostRequest(Guid learningKey, ReleaseEarningsRequest data) : IPostApiRequest<ReleaseEarningsRequest>
{
    public string PostUrl { get; } = $"learning/{learningKey}/release-earnings";
    public ReleaseEarningsRequest Data { get; set; } = data;
}

public class ReleaseEarningsRequest
{
    public Guid LearnerKey { get; set; }
    public string LearnerRef { get; set; } = string.Empty;
}
