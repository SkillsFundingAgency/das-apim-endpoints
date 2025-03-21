using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Jobs
{
    public class RequestJobRunApiRequest : IPostApiRequest
    {

        public RequestJobRunApiRequest()
        {
        }

        public object Data { get; set; }

        public string PostUrl => $"/api/job/requestrun";
    }
}
