using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Types;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class ValidateLearnerApiRequest : SaveDataRequest
{
    public long ProviderId { get; set; }
    public long LearnerDataId { get; set; }
    public GetLearnerForProviderResponse Learner { get; set; }
    public ProviderStandardsData ProviderStandardsData { get; set; }
}