using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command
{
    public class CreateOverlappingTrainingDateRequestCommand : IRequest<CreateOverlappingTrainingDateResult>
    {
        public long ProviderId { get; set; }
        public long DraftApprneticeshipId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
