using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetApprenticeFeedbackSummaryRequest : IGetAllApiRequest
    {
        public string GetAllUrl => $"api/apprenticefeedbackresult/reviews";
    }

}