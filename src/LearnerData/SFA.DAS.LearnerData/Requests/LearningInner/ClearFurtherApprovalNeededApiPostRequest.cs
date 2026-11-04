using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Common.Domain.Types;

namespace SFA.DAS.LearnerData.Requests.LearningInner;

public class ClearFurtherApprovalNeededApiPostRequest(Guid learningKey, Guid episodeKey, LearningType learningType) : IPostApiRequest
{
    public string PostUrl { get; } = $"learning/{learningKey}/episodes/{episodeKey}/clear-further-approval-needed?learningType={learningType}";
    public object Data { get; set; } = null!;
}
