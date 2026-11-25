using SFA.DAS.Apim.Shared.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class GetRolloverCandidatesForExportApiRequest : IGetApiRequest
{
    public Guid RolloverWorkflowRunId { get; set; }
    public string GetUrl => $"api/rollover/{RolloverWorkflowRunId}/rollovercandidatesforexport";
}