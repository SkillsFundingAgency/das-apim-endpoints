using SFA.DAS.SharedOuterApi.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

[ExcludeFromCodeCoverage]
public class GetRolloverWorkflowCandidatesApiRequest : IGetApiRequest
{
    public string GetUrl => "api/rollover/rolloverworkflowcandidates";
}
