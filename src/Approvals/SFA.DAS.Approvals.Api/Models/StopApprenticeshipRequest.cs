using System;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models
{
    public class StopApprenticeshipRequest
    {
        public long AccountId { get; set; }
        public DateTime StopDate { get; set; }
        public bool MadeRedundant { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
