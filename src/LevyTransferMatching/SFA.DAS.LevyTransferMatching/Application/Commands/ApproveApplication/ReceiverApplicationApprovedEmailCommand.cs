using MediatR;

namespace SFA.DAS.LevyTransferMatching.Application.Commands.ApproveApplication
{
    public class ReceiverApplicationApprovedEmailCommand : IRequest
    { 
        public int PledgeId { get; set; }
        public int ApplicationId { get; set; }
        public long ReceiverId { get; set; }
        public string BaseUrl { get; set; }
        public string ReceiverEncodedAccountId { get; set; }
    }
}
