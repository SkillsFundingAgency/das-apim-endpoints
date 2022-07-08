using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.SelectDeliveryModel
{
    public class GetSelectDeliveryModelQueryHandler : IRequestHandler<GetSelectDeliveryModelQuery, GetSelectDeliveryModelResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly IDeliveryModelService _deliveryModelService;

        public GetSelectDeliveryModelQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, IDeliveryModelService deliveryModelService)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _deliveryModelService = deliveryModelService;
        }

        public async Task<GetSelectDeliveryModelResult> Handle(GetSelectDeliveryModelQuery request, CancellationToken cancellationToken)
        {
            var apprenticeship = await _commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));

            if (apprenticeship == null || apprenticeship.ProviderId != request.ProviderId)
            {
                return null;
            }

            var deliveryModels = await _deliveryModelService.GetDeliveryModels(apprenticeship.ProviderId, apprenticeship.CourseCode,request.AccountLegalEntityId);

            return new GetSelectDeliveryModelResult
            {
                ApprenticeshipId = request.ApprenticeshipId,
                LegalEntityName = apprenticeship.EmployerName,
                DeliveryModels = deliveryModels
            };
        }
    }
}