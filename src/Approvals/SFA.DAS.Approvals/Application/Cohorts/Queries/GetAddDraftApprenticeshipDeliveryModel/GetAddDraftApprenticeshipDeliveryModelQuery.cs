using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDeliveryModel
{
    public class GetAddDraftApprenticeshipDeliveryModelQuery : IRequest<GetAddDraftApprenticeshipDeliveryModelQueryResult>
    {
        public long AccountLegalEntityId { get; set; }
        public long? ProviderId { get; set; }
        public string CourseCode { get; set; }
    }

    public class GetAddDraftApprenticeshipDeliveryModelQueryResult
    {
        public List<string> DeliveryModels { get; set; }
        public string EmployerName { get; set; }
    }

    public class GetAddDraftApprenticeshipDeliveryModelQueryHandler : IRequestHandler<GetAddDraftApprenticeshipDeliveryModelQuery, GetAddDraftApprenticeshipDeliveryModelQueryResult>
    {
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;

        public GetAddDraftApprenticeshipDeliveryModelQueryHandler(IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient)
        {
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
            _apiClient = apiClient;
        }

        public async Task<GetAddDraftApprenticeshipDeliveryModelQueryResult> Handle(GetAddDraftApprenticeshipDeliveryModelQuery request, CancellationToken cancellationToken)
        {
            var innerApiRequest = new GetAccountLegalEntityRequest(request.AccountLegalEntityId);
            var accountLegalEntityResult = await _apiClient.GetWithResponseCode<GetAccountLegalEntityResponse>(innerApiRequest);

            accountLegalEntityResult.EnsureSuccessStatusCode();

            var accountLegalEntity = accountLegalEntityResult.Body;

            var providerId = _serviceParameters.CallingParty == Party.Provider
                ? _serviceParameters.CallingPartyId
                : request.ProviderId.Value;

            var deliveryModels = await _deliveryModelService.GetDeliveryModels(providerId,
                request.CourseCode, request.AccountLegalEntityId, null);

            return new GetAddDraftApprenticeshipDeliveryModelQueryResult
            {
                DeliveryModels = deliveryModels,
                EmployerName = accountLegalEntity.LegalEntityName
            };
        }
    }
}
