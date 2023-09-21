using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationOutcome
{
    public class ApplicationCreatedEmailCommand : IRequest
    { 
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public long ReceiverId { get; set; }
        public string EncodedApplicationId { get; set; }
    }
}
