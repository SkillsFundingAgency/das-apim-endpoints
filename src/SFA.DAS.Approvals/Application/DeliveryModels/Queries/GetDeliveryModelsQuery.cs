using MediatR;

namespace SFA.DAS.Approvals.Application.DeliveryModels.Queries
{
    public class GetDeliveryModelsQuery : IRequest<GetDeliveryModelsQueryResult>
    {
        public long ProviderId { get; }
        public string TrainingCode { get; }
        public long AccountLegalEntityId { get; }

        public GetDeliveryModelsQuery(long providerId, string trainingCode, long accountLegalEntityId)
        {
            ProviderId = providerId;
            TrainingCode = trainingCode;
            AccountLegalEntityId = accountLegalEntityId;
        }
    }
}