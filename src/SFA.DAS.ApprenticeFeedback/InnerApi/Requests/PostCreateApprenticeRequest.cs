using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class PostCreateApprenticeRequest : IPostApiRequest
    {
        public string PostUrl => "api/add-apprentice";

        public object Data { get; set; }

        public PostCreateApprenticeRequest(object data)
        {
            Data = data;
        }
    }

    public class AddApprenticeData
    {
        public Guid ApprenticeId { get; set; }
        public long ApprenticeshipId { get; set; }
        public int? Status { get; set; }
    }
}
