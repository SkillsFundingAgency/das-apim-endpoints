using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Models;

public class InnerApiResponses
{
    public GetLearningsResponse LearningsInnerApiResponse { get; set; } = new();
    public GetFm36DataResponse EarningsInnerApiResponse { get; set; } = new();
}
