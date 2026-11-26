using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

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
