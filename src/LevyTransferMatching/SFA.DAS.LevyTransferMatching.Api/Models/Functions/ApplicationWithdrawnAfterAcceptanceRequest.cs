using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class ApplicationWithdrawnAfterAcceptanceRequest
    {
        public int ApplicationId { get; set; }
        public int PledgeId { get; set; }
        public int Amount { get; set; }
    }
}
