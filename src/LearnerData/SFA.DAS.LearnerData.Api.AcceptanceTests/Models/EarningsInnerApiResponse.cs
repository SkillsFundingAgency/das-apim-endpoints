using SFA.DAS.LearnerData.Application.Fm36;
using SFA.DAS.LearnerData.Requests;
using SFA.DAS.LearnerData.Responses.EarningsInner;
using SFA.DAS.LearnerData.Responses.LearningInner;

namespace SFA.DAS.LearnerData.Api.AcceptanceTests.Models;

using Learning = SFA.DAS.LearnerData.Responses.LearningInner.Learning;

public class InnerApiResponses
{
    public List<GetPagedLearnersFromLearningInner> PagedLearningsInnerApiResponse { get; set; } = new();
    public List<Learning> UnPagedLearningsInnerApiResponse { get; set; } = new();
    public GetFm36DataResponse EarningsInnerApiResponse { get; set; } = new();
    public List<UpdateLearnerRequest> SldLearnerData { get; set; } = new();
}