using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class PostApprenticeFeedbackRatingRequest : IPostApiRequest
    {
        public string PostUrl => $"api/apprenticefeedback/ratings";
        public object Data { get; set; }
    }

    public class PostApprenticeFeedbackRatingRequestData
    {
        public IEnumerable<int> Ukprns { get; set; }
    }
}