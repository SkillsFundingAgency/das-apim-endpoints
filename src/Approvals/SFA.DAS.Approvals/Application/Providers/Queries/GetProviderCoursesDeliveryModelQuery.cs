using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProviderCoursesDeliveryModelQuery : IRequest<GetProviderCourseDeliveryModelsResponse>
    {
        public long ProviderId { get; }
        public string TrainingCode { get; }

        public GetProviderCoursesDeliveryModelQuery(long providerId, string trainingCode)
        {
            ProviderId = providerId;
            TrainingCode = trainingCode;
        }
    }
}