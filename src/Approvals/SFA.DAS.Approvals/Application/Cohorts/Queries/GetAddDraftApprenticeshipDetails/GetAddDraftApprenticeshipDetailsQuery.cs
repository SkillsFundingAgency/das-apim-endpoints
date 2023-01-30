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

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetAddDraftApprenticeshipDetails
{
    public class GetAddDraftApprenticeshipDetailsQuery : IRequest<GetAddDraftApprenticeshipDetailsQueryResult>
    {
        public long AccountLegalEntityId { get; set; }
        public long? ProviderId { get; set; }
        public string CourseCode { get; set; }
    }

    public class GetAddDraftApprenticeshipDetailsQueryResult
    {
        public string LegalEntityName { get; set; }
        public string ProviderName { get; set; }
        public bool HasMultipleDeliveryModelOptions { get; set; }
    }

    public class GetAddDraftApprenticeshipDetailsQueryHandler : IRequestHandler<GetAddDraftApprenticeshipDetailsQuery, GetAddDraftApprenticeshipDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ServiceParameters _serviceParameters;

        public GetAddDraftApprenticeshipDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IDeliveryModelService deliveryModelService, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _deliveryModelService = deliveryModelService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetAddDraftApprenticeshipDetailsQueryResult> Handle(GetAddDraftApprenticeshipDetailsQuery request, CancellationToken cancellationToken)
        {
            var providerId = _serviceParameters.CallingParty == Shared.Enums.Party.Employer
                ? request.ProviderId.Value
                : _serviceParameters.CallingPartyId;

            var providerResponseTask = _apiClient.GetWithResponseCode<GetProviderResponse>(new GetProviderRequest(providerId));
            var accountLegalEntityResponseTask = _apiClient.GetWithResponseCode<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(request.AccountLegalEntityId));

            await Task.WhenAll(providerResponseTask, accountLegalEntityResponseTask);

            if (providerResponseTask.Result.StatusCode == HttpStatusCode.NotFound || accountLegalEntityResponseTask.Result.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            providerResponseTask.Result.EnsureSuccessStatusCode();
            accountLegalEntityResponseTask.Result.EnsureSuccessStatusCode();

            var provider = providerResponseTask.Result.Body;
            var accountLegalEntity = accountLegalEntityResponseTask.Result.Body;

            if (provider == null || accountLegalEntity == null) return null;

            if (!CheckParty(accountLegalEntity))
            {
                return null;
            }

            var deliveryModels = await _deliveryModelService.GetDeliveryModels(providerId,
                request.CourseCode, request.AccountLegalEntityId, null);

            return new GetAddDraftApprenticeshipDetailsQueryResult
            {
                LegalEntityName = accountLegalEntity.LegalEntityName,
                ProviderName = provider.Name,
                HasMultipleDeliveryModelOptions = deliveryModels.Count > 1
            };
        }

        private bool CheckParty(GetAccountLegalEntityResponse accountLegalEntity)
        {
            switch (_serviceParameters.CallingParty)
            {
                case Shared.Enums.Party.Employer:
                    {
                        if (accountLegalEntity.AccountId != _serviceParameters.CallingPartyId)
                        {
                            return false;
                        }

                        break;
                    }
                case Shared.Enums.Party.Provider:
                {
                    return true;
                }
                default:
                    return false;
            }

            return true;
        }
    }
}
