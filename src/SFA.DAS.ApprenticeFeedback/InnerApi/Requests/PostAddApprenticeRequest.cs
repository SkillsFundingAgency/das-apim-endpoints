using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.ApprenticeFeedback.InnerApi.Requests
{
    public class PostAddApprenticeRequest : IPostApiRequest
    {
        public string PostUrl => "api/add-apprentice";

        public object Data { get; set; }

        public PostAddApprenticeRequest(object data)
        {
            Data = data;
        }
    }

    public class AddApprenticeData
    {
        public Guid ApprenticeId { get; set; }
        public Guid ApprenticeshipId { get; set; }
        public string Status { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
    }
}
