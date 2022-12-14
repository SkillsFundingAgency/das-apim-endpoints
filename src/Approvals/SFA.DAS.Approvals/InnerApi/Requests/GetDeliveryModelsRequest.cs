using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetDeliveryModelsRequest : IGetApiRequest
    {
        public long ProviderId { get; }
        public string TrainingCode { get; }

        public GetDeliveryModelsRequest(long providerId, string courseCode)
        {
            ProviderId = providerId;
            TrainingCode = courseCode;
        }
        public string GetUrl => $"api/providers/{ProviderId}/courses/{TrainingCode}";
    }
}