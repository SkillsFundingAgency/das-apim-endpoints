using MediatR;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Application.Providers.Queries
{
    public class GetProviderCoursesDeliveryModelQuery : IRequest<GetProviderCourseDeliveryModelsResponse>
    {
        public long ProviderId { get; }
        public string TrainingCode { get; }
        public long AccountLegalEntityId { get; }
        public string EncodedAccountId { get; }

        public GetProviderCoursesDeliveryModelQuery(long providerId, string trainingCode, string encodedAccountId, long accountLegalEntityId)
        {
            ProviderId = providerId;
            TrainingCode = trainingCode;
            EncodedAccountId = encodedAccountId;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}