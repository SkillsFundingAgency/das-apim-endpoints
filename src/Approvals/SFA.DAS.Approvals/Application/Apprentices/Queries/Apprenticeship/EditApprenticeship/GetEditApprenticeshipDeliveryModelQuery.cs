using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.EditApprenticeship
{
    public class GetEditApprenticeshipDeliveryModelQuery : IRequest<GetEditApprenticeshipDeliveryModelQueryResult>
    {
        public long ApprenticeshipId { get; set; }
    }

    public class GetEditApprenticeshipDeliveryModelQueryResult
    {
        public long ApprenticeshipId { get; set; }
        public string LegalEntityName { get; set; }
        public List<string> DeliveryModels { get; set; }
    }

    public class GetEditApprenticeshipDeliveryModelQueryHandler : IRequestHandler<GetEditApprenticeshipDeliveryModelQuery, GetEditApprenticeshipDeliveryModelQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;

        public GetEditApprenticeshipDeliveryModelQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetEditApprenticeshipDeliveryModelQueryResult> Handle(GetEditApprenticeshipDeliveryModelQuery request, CancellationToken cancellationToken)
        {
            var innerApiRequest = new GetApprenticeshipRequest(request.ApprenticeshipId);
            var innerApiResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipResponse>(innerApiRequest);

            if (innerApiResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            innerApiResponse.EnsureSuccessStatusCode();
            var apprenticeship = innerApiResponse.Body;

            if (!CheckParty(apprenticeship))
            {
                return null;
            }

            var deliveryModels = await _deliveryModelService.GetDeliveryModels(apprenticeship);

            return new GetEditApprenticeshipDeliveryModelQueryResult
            {
                ApprenticeshipId = request.ApprenticeshipId,
                LegalEntityName = apprenticeship.EmployerName,
                DeliveryModels = deliveryModels
            };
        }

        private bool CheckParty(GetApprenticeshipResponse apprenticeship)
        {
            switch (_serviceParameters.CallingParty)
            {
                case Party.Employer:
                {
                    if (apprenticeship.EmployerAccountId != _serviceParameters.CallingPartyId)
                    {
                        return false;
                    }

                    break;
                }
                case Party.Provider:
                {
                    if (apprenticeship.ProviderId != _serviceParameters.CallingPartyId)
                    {
                        return false;
                    }

                    break;
                }
                default:
                    return false;
            }

            return true;
        }
    }
}
