using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public class GetRolloverCandidatesForExportApiRequest : IGetApiRequest
{
    public Guid RolloverWorkflowRunId { get; set; }
    public string GetUrl => $"api/rollover/{RolloverWorkflowRunId}/rollovercandidatesforexport";
}