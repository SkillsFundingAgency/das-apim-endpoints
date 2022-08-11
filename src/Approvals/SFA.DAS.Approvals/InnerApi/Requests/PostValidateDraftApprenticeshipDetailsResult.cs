using SFA.DAS.Approvals.Application.OverlappingTrainingDateRequest.Command;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class PostValidateDraftApprenticeshipDetailsRequest : IPostApiRequest
    {
        public readonly long ProviderId;
        public object Data { get; set; }

        public string PostUrl => $"api/overlapping-training-date-request/{ProviderId}/validate";

        public PostValidateDraftApprenticeshipDetailsRequest(ValidateDraftApprenticeshipRequest request)
        {
            ProviderId = request.ProviderId;
            Data = request;
        }
    }
}
