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

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetAddDraftApprenticeshipDetails
{
    public class GetAddDraftApprenticeshipDetailsQuery : IRequest<GetAddDraftApprenticeshipDetailsQueryResult>
    {
        public long CohortId { get; set; }
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
            var cohortRequest = new GetCohortRequest(request.CohortId);
            var cohortResponse = await _apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);

            if (cohortResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            cohortResponse.EnsureSuccessStatusCode();

            var cohort = cohortResponse.Body;

            if (!CheckParty(cohort))
            {
                return null;
            }

            var deliveryModels = await _deliveryModelService.GetDeliveryModels(cohort.ProviderId,
                request.CourseCode, cohort.AccountLegalEntityId, null);

            return new GetAddDraftApprenticeshipDetailsQueryResult
            {
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                HasMultipleDeliveryModelOptions = deliveryModels.Count > 1
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
