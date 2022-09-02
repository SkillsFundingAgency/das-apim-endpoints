using System;
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
    }

    public class GetCohortDetailsQueryHandler : IRequestHandler<GetCohortDetailsQuery, GetCohortDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly ServiceParameters _serviceParameters;

        public GetCohortDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetCohortDetailsQueryResult> Handle(GetCohortDetailsQuery request, CancellationToken cancellationToken)
        {
            var cohortRequest = new GetCohortRequest(request.CohortId);

            var cohortResponseTask = await _apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);


            if (cohortResponseTask.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            cohortResponseTask.EnsureSuccessStatusCode();

            var cohort = cohortResponseTask.Body;

            if (!CheckParty(cohort))
            {
                return null;
            }

            return new GetCohortDetailsQueryResult
            {
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName
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
