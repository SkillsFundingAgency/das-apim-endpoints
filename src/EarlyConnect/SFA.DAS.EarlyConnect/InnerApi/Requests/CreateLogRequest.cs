using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class CreateLogRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public CreateLogRequest(CreateLog createLog)
        {
            Data = createLog;
        }

        public string PostUrl => "api/log/add";
    }
}