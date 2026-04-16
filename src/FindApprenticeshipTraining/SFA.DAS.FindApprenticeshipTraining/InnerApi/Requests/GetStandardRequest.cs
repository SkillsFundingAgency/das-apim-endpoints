using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
public class GetStandardRequest : IGetApiRequest
{
    public GetStandardRequest(string id)
    {
        StandardId = id;
    }

    public string StandardId { get; }
    public string GetUrl => $"api/courses/standards/{StandardId}";
}
