using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class GetApprenticeFeedbackTargetsForUpdateRequest : IGetAllApiRequest
    {
        public int BatchSize { get; set; }
        public string GetAllUrl => $"api/apprenticefeedbacktarget/requiresupdate?batchSize={BatchSize}";
    }
}
