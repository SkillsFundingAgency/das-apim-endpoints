using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.RejectApplication
{
    public class ApplicationRejectedEmailCommand : IRequest<Unit>
    { 
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public long ReceiverId { get; set; }
        public string BaseUrl { get; set; }
        public string EncodedApplicationId { get; set; }
    }
}
