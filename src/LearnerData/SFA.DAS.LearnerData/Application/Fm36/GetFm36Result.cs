using ESFA.DC.ILR.FundingService.FM36.FundingOutput.Model.Output;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

namespace SFA.DAS.LearnerData.Application.Fm36;

public class GetFm36Result : PagedQueryResult<FM36Learner>
{

}

public class GetPagedLearnersFromLearningInner : PagedQueryResult<Learning>
{

}