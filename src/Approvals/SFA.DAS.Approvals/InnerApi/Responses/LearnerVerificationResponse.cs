using System.Collections.Generic;
using SFA.DAS.Approvals.Enums;

namespace SFA.DAS.Approvals.InnerApi.Responses
{
    public class LearnerVerificationResponse
    {
        public LearnerVerificationResponseCode ResponseCode { get; set; }
        public IEnumerable<FailureFlag>? FailureFlags { get; set; }
    }
}