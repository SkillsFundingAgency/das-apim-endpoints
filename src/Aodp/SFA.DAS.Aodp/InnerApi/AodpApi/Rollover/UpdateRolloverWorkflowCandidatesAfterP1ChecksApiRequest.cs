using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover
{
    public class UpdateRolloverWorkflowCandidatesAfterP1ChecksApiRequest : IPostApiRequest
    {
        public string PostUrl => $"api/rollover/p1checks";

        public object Data { get; set; } = new { };
    }
}