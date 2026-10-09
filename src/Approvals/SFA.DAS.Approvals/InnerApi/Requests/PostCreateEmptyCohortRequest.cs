using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests;

public class PostCreateEmptyCohortRequest : IPostApiRequest
{
    public object Data { get; set; }

    public string PostUrl => $"api/cohorts/create-empty-cohort";

    public PostCreateEmptyCohortRequest(CreateEmptyCohortRequest data)
    {
        Data = data;
    }
}
