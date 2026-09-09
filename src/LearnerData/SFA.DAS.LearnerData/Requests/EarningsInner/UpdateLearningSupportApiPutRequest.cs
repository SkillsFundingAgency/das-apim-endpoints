using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.LearnerData.Shared;

namespace SFA.DAS.LearnerData.Requests.EarningsInner;

public class UpdateLearningSupportApiPutRequest(Guid learningKey, UpdateLearningSupportRequest data) : IPutApiRequest<UpdateLearningSupportRequest>
{
    public string PutUrl { get; } = $"learning/{learningKey}/learning-support";

    public UpdateLearningSupportRequest Data { get; set; } = data;
}

public class UpdateLearningSupportRequest
{
    public List<LearningSupport> LearningSupport { get; set; } = [];
}
