using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover;

public class GetRolloverWorkflowCandidatesApiRequest : IGetApiRequest
{
    public int? Skip { get; set; }
    public int? Take { get; set; }

    public GetRolloverWorkflowCandidatesApiRequest(int? skip, int? take)
    {
        Skip = skip;
        Take = take;
    }
    public string GetUrl => "api/rollover/workflowcandidates";
}
