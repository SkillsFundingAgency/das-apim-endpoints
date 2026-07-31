using SFA.DAS.Aodp.Application.Commands.Rollover;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover
{
    public class RemovePreviousWorkflowCandidatesApiRequest : IPostApiRequest
    {
        public string PostUrl => $"api/rollover/removepreviousworkflowcandidates";

        public object Data { get; set; } = new { };
    }
}
