using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.Api.Models.OverlappingTrainingDateRequest
{
    public class CreateOverlappingTrainingDateRequest
    {
        public long ProviderId { get; set; }
        public long DraftApprenticeshipId { get; set; }
        public UserInfo UserInfo { get; set; }
    }
}
