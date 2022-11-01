
using SFA.DAS.SharedOuterApi.Interfaces;


namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class GetEmployerFeedbackSummaryRequest : IGetAllApiRequest
    {
        public string GetAllUrl => $"api/employerfeedbackresult/reviews";
    }
}
