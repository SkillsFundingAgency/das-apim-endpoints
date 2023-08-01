using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetEditApprenticeshipCourse
{
    public class GetEditApprenticeshipCourseQueryHandler : IRequestHandler<GetEditApprenticeshipCourseQuery, GetEditApprenticeshipCourseQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;
        private readonly ServiceParameters _serviceParameters;

        public GetEditApprenticeshipCourseQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IProviderStandardsService providerStandardsService,
            ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetEditApprenticeshipCourseQueryResult> Handle(GetEditApprenticeshipCourseQuery request, CancellationToken cancellationToken)
        {
            var apprenticeshipResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));

            if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apprenticeshipResponse.EnsureSuccessStatusCode();

            var apprenticeship = apprenticeshipResponse.Body;

            if (!apprenticeship.CheckParty(_serviceParameters))
            {
                return null;
            }

            var providerStandardsData = await _providerStandardsService.GetStandardsData(apprenticeship.ProviderId);

            return new GetEditApprenticeshipCourseQueryResult
            {
                EmployerName = apprenticeship.EmployerName,
                ProviderName = apprenticeship.ProviderName,
                IsMainProvider = providerStandardsData.IsMainProvider,
                Standards = providerStandardsData.Standards.Select(x =>
                    new GetEditApprenticeshipCourseQueryResult.Standard
                    { CourseCode = x.CourseCode, Name = x.Name })
            };
        }
    }
}