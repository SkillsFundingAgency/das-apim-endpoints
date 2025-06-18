using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;

namespace SFA.DAS.Earnings.Api.AcceptanceTests.Models;

public class InnerApiResponses
{
    public GetLearningsResponse LearningsInnerApiResponse { get; set; } = new();
    public GetFm36DataResponse EarningsInnerApiResponse { get; set; } = new();
}
