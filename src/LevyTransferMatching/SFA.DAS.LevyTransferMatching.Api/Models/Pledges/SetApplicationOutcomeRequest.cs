using SFA.DAS.LevyTransferMatching.Types;

namespace SFA.DAS.LevyTransferMatching.Api.Models.Pledges
{
    public class SetApplicationOutcomeRequest
    {
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }

        public ApplicationOutcome Outcome { get; set; }
    }
}
