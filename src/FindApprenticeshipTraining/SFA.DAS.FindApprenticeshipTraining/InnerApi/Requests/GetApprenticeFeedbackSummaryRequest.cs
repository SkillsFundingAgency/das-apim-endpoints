using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetApprenticeFeedbackSummaryRequest : IGetAllApiRequest
    {
        public string GetAllUrl => $"api/apprenticefeedbackresult/reviews";
    }

}