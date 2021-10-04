using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class AcceptFundingRequest
    {
        public int ApplicationId { get; set; }
        public long AccountId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
    }
}
