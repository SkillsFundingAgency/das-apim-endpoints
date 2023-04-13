using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipCourse
{
    public class GetEditDraftApprenticeshipCourseQueryHandler : IRequestHandler<GetEditDraftApprenticeshipCourseQuery, GetEditDraftApprenticeshipCourseQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;

        public GetEditDraftApprenticeshipCourseQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, IProviderStandardsService providerStandardsService)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
        }

        public async Task<GetEditDraftApprenticeshipCourseQueryResult> Handle(GetEditDraftApprenticeshipCourseQuery request, CancellationToken cancellationToken)
        {
            var cohort = await _apiClient.Get<GetCohortResponse>(new GetCohortRequest(request.CohortId));

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