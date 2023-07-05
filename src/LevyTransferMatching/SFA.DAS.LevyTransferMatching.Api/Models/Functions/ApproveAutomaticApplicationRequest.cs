using SFA.DAS.LevyTransferMatching.Types;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Functions
{
    public class ApproveAutomaticApplicationRequest
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }       
    }
}
