using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProviderCoursesDeliveryModelQuery : IRequest<GetProviderCourseDeliveryModelsResponse>
    {
        public long ProviderId { get; }
        public string TrainingCode { get; }
        public int LegalEntityId { get; }

        public GetProviderCoursesDeliveryModelQuery(long providerId, string trainingCode, int legalEntityId)
        {
            ProviderId = providerId;
            TrainingCode = trainingCode;
            LegalEntityId = legalEntityId;
        }
    }
}