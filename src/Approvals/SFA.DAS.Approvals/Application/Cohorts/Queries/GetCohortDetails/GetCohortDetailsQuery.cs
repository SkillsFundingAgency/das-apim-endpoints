using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Application.DeliveryModels.Constants;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails
{
    public class GetCohortDetailsQuery : IRequest<GetCohortDetailsQueryResult>
    {
        public long CohortId { get; set; }
    }

    public class GetCohortDetailsQueryResult
    {
        public string ProviderName { get; set; }
        public string LegalEntityName { get; set; }
        public bool HasUnavailableFlexiJobAgencyDeliveryModel { get; set; }
    }

    public class GetCohortDetailsQueryHandler : IRequestHandler<GetCohortDetailsQuery, GetCohortDetailsQueryResult>
    {
        private readonly IDeliveryModelService _deliveryModelService;
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly ServiceParameters _serviceParameters;
        private readonly IFjaaApiClient<FjaaApiConfiguration> _fjaaClient;

        public GetCohortDetailsQueryHandler(IDeliveryModelService deliveryModelService, ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, ServiceParameters serviceParameters)
        {
            _deliveryModelService = deliveryModelService;
            _apiClient = apiClient;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetCohortDetailsQueryResult> Handle(GetCohortDetailsQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new GetDraftApprenticeshipsRequest(request.CohortId);
            var cohortRequest = new GetCohortRequest(request.CohortId);

            var apiResponseTask = _apiClient.GetWithResponseCode<GetDraftApprenticeshipsResponse>(apiRequest);
            var cohortResponseTask = _apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);

            await Task.WhenAll(apiResponseTask, cohortResponseTask);

            if (apiResponseTask.Result.StatusCode == HttpStatusCode.NotFound || cohortResponseTask.Result.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apiResponseTask.Result.EnsureSuccessStatusCode();
            cohortResponseTask.Result.EnsureSuccessStatusCode();

            var apprenticeships = apiResponseTask.Result.Body;
            var cohort = cohortResponseTask.Result.Body;

            var isOnRegister = await _deliveryModelService.IsLegalEntityOnFjaaRegister(cohort.AccountLegalEntityId);

            if (!CheckParty(cohort))
            {
                return null;
            }

            return new GetCohortDetailsQueryResult
            {
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                HasUnavailableFlexiJobAgencyDeliveryModel = isOnRegister
            };
        }

        private bool CheckParty(GetCohortResponse cohort)
        {
            switch (_serviceParameters.CallingParty)
            {
                case Party.Employer:
                {
                    if (cohort.AccountId != _serviceParameters.CallingPartyId)
                    {
                        return false;
                    }

                    break;
                }
                case Party.Provider:
                {
                    if (cohort.ProviderId != _serviceParameters.CallingPartyId)
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
