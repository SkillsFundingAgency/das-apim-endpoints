using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests
{
    public class PostApprenticeFeedbackRequest : IPostApiRequest
    {
        public string PostUrl => $"api/apprenticefeedbackresult/request";
        public object Data { get; set; }
    }

    public class PostApprenticeFeedbackRequestData
    {
        public IEnumerable<int> Ukprns { get; set; }
    }
}