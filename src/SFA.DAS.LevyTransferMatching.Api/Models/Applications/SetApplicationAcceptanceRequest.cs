using SFA.DAS.LevyTransferMatching.Types;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Applications
{
    public class SetApplicationAcceptanceRequest
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public ApplicationAcceptance Acceptance { get; set; }
    }
}