using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProviderCoursesDeliveryModelQuery : IRequest<GetProviderCourseDeliveryModelsResponse>
    {
        public long ProviderId { get; }
        public string TrainingCode { get; }
        public long AccountLegalEntityId { get; }

        public GetProviderCoursesDeliveryModelQuery(long providerId, string trainingCode, long accountLegalEntityId)
        {
            ProviderId = providerId;
            TrainingCode = trainingCode;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}