using SFA.DAS.Aodp.Application.Commands.Rollover;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Rollover
{
    public class CreateRolloverWorkflowRunApiRequest : IPostApiRequest
    {
        public string PostUrl => $"api/rollover/rolloverworkflowruns";

        public object Data { get; set; }
    }
}