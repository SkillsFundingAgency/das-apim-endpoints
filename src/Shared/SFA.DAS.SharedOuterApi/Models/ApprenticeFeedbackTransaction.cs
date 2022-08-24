
using System;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;


namespace SFA.DAS.SharedOuterApi.Models
{
    public class ApprenticeFeedbackTransaction
    {
        public long Id { get; set; }
        public Guid ApprenticeId { get; set; }
    }
}
