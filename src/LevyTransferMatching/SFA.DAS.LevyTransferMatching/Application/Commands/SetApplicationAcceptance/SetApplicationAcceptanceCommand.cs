using MediatR;
using SFA.DAS.LevyTransferMatching.Types;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationAcceptance
{
    public class SetApplicationAcceptanceCommand : IRequest<bool>
    {
        public long AccountId { get; set; }
        public int ApplicationId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public ApplicationAcceptance Acceptance { get; set; }
    }
}