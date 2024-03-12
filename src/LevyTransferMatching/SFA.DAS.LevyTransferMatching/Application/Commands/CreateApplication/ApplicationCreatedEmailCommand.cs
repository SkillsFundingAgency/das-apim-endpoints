using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication
{
    public class ApplicationCreatedEmailCommand : IRequest<Unit>
    { 
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public long ReceiverId { get; set; }
        public string EncodedApplicationId { get; set; }
    }
}
