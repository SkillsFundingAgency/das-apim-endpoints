using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Services;

namespace SFA.DAS.Approvals.Application.DeliveryModels.Queries
{
    public class GetDeliveryModelsQueryHandler : IRequestHandler<GetDeliveryModelsQuery, GetDeliveryModelsQueryResult>
    {
        private readonly IDeliveryModelService _deliveryModelService;

        public GetDeliveryModelsQueryHandler(IDeliveryModelService deliveryModelService)
        {
            _deliveryModelService = deliveryModelService;
        }

        public async Task<GetDeliveryModelsQueryResult> Handle(GetDeliveryModelsQuery request, CancellationToken cancellationToken)
        {
            var deliveryModels = await _deliveryModelService.GetDeliveryModels(request.ProviderId, request.TrainingCode, request.AccountLegalEntityId);
            return new GetDeliveryModelsQueryResult { DeliveryModels = deliveryModels };
        }
    }
}