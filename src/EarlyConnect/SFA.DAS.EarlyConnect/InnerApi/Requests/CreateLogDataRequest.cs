using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class CreateLogDataRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateLogDataRequest(LogCreate createLog)
        {
            Data = createLog;
        }

        public string PostUrl => "api/log/add";
    }
}