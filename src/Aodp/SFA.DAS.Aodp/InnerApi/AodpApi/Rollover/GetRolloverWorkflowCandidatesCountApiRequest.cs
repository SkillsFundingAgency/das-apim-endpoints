using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class GetRolloverWorkflowCandidatesCountApiRequest : IGetApiRequest
{
    public string GetUrl => "api/rollover/workflowcandidatescount";
}
