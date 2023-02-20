using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostCreateCohortRequest : IPostApiRequest
    {
        public object Data { get; set; }

        public string PostUrl => $"api/cohorts";

        public PostCreateCohortRequest(CreateCohortRequest data)
        {
            Data = data;
        }
    }
}
