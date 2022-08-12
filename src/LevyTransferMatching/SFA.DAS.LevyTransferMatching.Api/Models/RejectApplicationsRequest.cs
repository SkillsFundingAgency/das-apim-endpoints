using System.Collections.Generic;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class RejectApplicationsRequest
    {
        public List<int> ApplicationsToReject { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}