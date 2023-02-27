using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Learners
{
    public class VerifyLearnerValidationResponseQueryRequest : IPostApiRequest
    {
        public VerifyLearnerValidationResponseQueryRequest(Body body)
        {
            Data = body;
        }

        public string PostUrl => $"api/learners/validate-learner-verification";

        public object Data { get; set; }

        public class Body
        {
            public string SearchedULN { get; set; }
            public string ResponseCode { get; set; }
            public string Uln { get; set; }
            public string[] FailureFlag { get; set; }
        }
    }
}