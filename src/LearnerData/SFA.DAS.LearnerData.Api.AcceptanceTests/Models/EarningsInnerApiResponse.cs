using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Earnings;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Models;

public class InnerApiResponses
{
    public List<GetPagedLearnersFromLearningInner> PagedLearningsInnerApiResponse { get; set; } = new();
    public List<Learning> UnPagedLearningsInnerApiResponse { get; set; } = new();
    public GetFm36DataResponse EarningsInnerApiResponse { get; set; } = new();
    public List<UpdateLearnerRequest> SldLearnerData { get; set; } = new();
}