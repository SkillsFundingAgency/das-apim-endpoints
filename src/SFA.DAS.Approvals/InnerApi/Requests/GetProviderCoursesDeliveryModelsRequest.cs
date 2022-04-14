using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.InnerApi.Requests
{
    public class GetProviderCoursesDeliveryModelsRequest : IGetApiRequest
    {
        public long ProviderId { get; }
        public string TrainingCode { get; }

        public GetProviderCoursesDeliveryModelsRequest(long providerId, string courseCode)
        {
            ProviderId = providerId;
            TrainingCode = courseCode;
        }
        public string GetUrl => $"providers/{ProviderId}/courses/{TrainingCode}";
    }
}