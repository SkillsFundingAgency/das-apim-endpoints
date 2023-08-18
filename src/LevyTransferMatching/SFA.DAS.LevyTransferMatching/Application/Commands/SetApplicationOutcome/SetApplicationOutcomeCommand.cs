using MediatR;
using SFA.DAS.LevyTransferMatching.Types;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome
{
    public class SetApplicationOutcomeCommand : IRequest
    {
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public ApplicationOutcome Outcome { get; set; }
    }
}
