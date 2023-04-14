using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse
{
    public class GetEditDraftApprenticeshipCourseQueryHandler : IRequestHandler<GetEditDraftApprenticeshipCourseQuery, GetEditDraftApprenticeshipCourseQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;
        private readonly ServiceParameters _serviceParameters;

        public GetEditDraftApprenticeshipCourseQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IProviderStandardsService providerStandardsService,
            ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetEditDraftApprenticeshipCourseQueryResult> Handle(GetEditDraftApprenticeshipCourseQuery request, CancellationToken cancellationToken)
        {
            var cohortResponse = await _apiClient.GetWithResponseCode<GetCohortResponse>(new GetCohortRequest(request.CohortId));

            if (cohortResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            cohortResponse.EnsureSuccessStatusCode();

            var cohort = cohortResponse.Body;

            if (!cohort.CheckParty(_serviceParameters))
            {
                return null;
            }

            var providerStandardsData = await _providerStandardsService.GetStandardsData(cohort.ProviderId);

            return new GetEditDraftApprenticeshipCourseQueryResult
            {
                EmployerName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                IsMainProvider = providerStandardsData.IsMainProvider,
                Standards = providerStandardsData.Standards.Select(x =>
                    new GetEditDraftApprenticeshipCourseQueryResult.Standard
                        { CourseCode = x.CourseCode, Name = x.Name })
            };
        }
    }
}