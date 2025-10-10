using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Models;

public class InnerApiResponses
{
    public List<GetPagedLearnersFromLearningInner> PagedLearningsInnerApiResponse { get; set; } = new();
    public List<Learning> UnPagedLearningsInnerApiResponse { get; set; } = new();
    public List<GetFm36DataResponse> EarningsInnerApiResponses { get; set; } = new();
}
