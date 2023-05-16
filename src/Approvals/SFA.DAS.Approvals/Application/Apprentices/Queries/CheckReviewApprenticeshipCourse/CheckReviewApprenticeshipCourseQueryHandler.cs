using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.CheckReviewApprenticeshipCourse
{
    public class CheckReviewApprenticeshipCourseQueryHandler : IRequestHandler<CheckReviewApprenticeshipCourseQuery, CheckReviewApprenticeshipCourseQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;
        private readonly ServiceParameters _serviceParameters;

        public CheckReviewApprenticeshipCourseQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IProviderStandardsService providerStandardsService,
            ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
            _serviceParameters = serviceParameters;
        }

        public async Task<CheckReviewApprenticeshipCourseQueryResult> Handle(CheckReviewApprenticeshipCourseQuery request, CancellationToken cancellationToken)
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

            var apprenticeshipUpdateResponse = await _apiClient.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(new GetApprenticeshipUpdatesRequest(request.ApprenticeshipId, Convert.ToByte(ApprenticeshipStatus.WaitingToStart)));

            if (apprenticeshipUpdateResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apprenticeshipUpdateResponse.EnsureSuccessStatusCode();

            var apprenticeshipUpdate = apprenticeshipUpdateResponse.Body;

            var update = apprenticeshipUpdate.ApprenticeshipUpdates.First();

            if (string.IsNullOrWhiteSpace(update.TrainingCode))
            {
                return null;
            }

            var providerStandardsData = await _providerStandardsService.GetStandardsData(apprenticeship.ProviderId);

            var standard = providerStandardsData.Standards.FirstOrDefault(x => int.Parse(x.CourseCode) == int.Parse(update.TrainingCode));

            return new CheckReviewApprenticeshipCourseQueryResult
            {
                IsValidCourseCode = standard != null
            };
        }
    }
}