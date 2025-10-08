using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Models;

public class InnerApiResponses
{
    public GetLearningsResponse LearningsInnerApiResponse { get; set; } = new();
    public List<GetFm36DataResponse> EarningsInnerApiResponses { get; set; } = new();
}
