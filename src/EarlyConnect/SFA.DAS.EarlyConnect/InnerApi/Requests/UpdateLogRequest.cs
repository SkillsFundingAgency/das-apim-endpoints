using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class UpdateLogRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public UpdateLogRequest(UpdateLog createLog)
        {
            Data = createLog;
        }

        public string PostUrl => "api/log/update";
    }
}