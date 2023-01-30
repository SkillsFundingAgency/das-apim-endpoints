using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
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
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly ServiceParameters _serviceParameters;
        private readonly IFjaaService _fjaaService;

        public GetCohortDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, ServiceParameters serviceParameters, IFjaaService fjaaService)
        {
            _apiClient = apiClient;
            _serviceParameters = serviceParameters;
            _fjaaService = fjaaService;
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

            if (!cohort.CheckParty(_serviceParameters))
            {
                return null;
            }

            var isOnRegister = await _fjaaService.IsAccountLegalEntityOnFjaaRegister(cohort.AccountLegalEntityId);

            return new GetCohortDetailsQueryResult
            {
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                HasUnavailableFlexiJobAgencyDeliveryModel = !isOnRegister && apprenticeships.DraftApprenticeships.Any(a => a.DeliveryModel.Equals(DeliveryModel.FlexiJobAgency))
            };
        }
    }
}
