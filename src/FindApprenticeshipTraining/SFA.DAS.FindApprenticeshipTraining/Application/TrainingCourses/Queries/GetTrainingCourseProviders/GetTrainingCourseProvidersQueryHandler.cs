using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler : IRequestHandler<GetTrainingCourseProvidersQuery, GetTrainingCourseProvidersResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IShortlistService _shortlistService;
        private readonly ILocationLookupService _locationLookupService;

        public GetTrainingCourseProvidersQueryHandler(
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IShortlistService shortlistService,
            ILocationLookupService locationLookupService)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _coursesApiClient = coursesApiClient;
            _shortlistService = shortlistService;
            _locationLookupService = locationLookupService;
        }

        public async Task<GetTrainingCourseProvidersResult> Handle(GetTrainingCourseProvidersQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.Location, request.Lat, request.Lon);

            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));

            var shortlistTask = _shortlistService.GetShortlistItemCount(request.ShortlistUserId);

            await Task.WhenAll(locationTask, courseTask, shortlistTask);

            var providers = await _courseDeliveryApiClient.Get<GetProvidersListResponse>(new GetProvidersByCourseRequest(
                request.Id,
                courseTask.Result.SectorSubjectAreaTier2Description,
                courseTask.Result.Level,
                locationTask.Result?.GeoPoint?.FirstOrDefault(),
                locationTask.Result?.GeoPoint?.LastOrDefault(),
                request.SortOrder,
                request.ShortlistUserId));

            if (providers?.Providers.Any() == true)
            {
                var ukprns = providers.Providers.Select(s => s.Ukprn);

                var apprenticeFeedbackRatings =
                    await _apprenticeFeedbackApiClient.
                    PostWithResponseCode<IEnumerable<GetApprenticeFeedbackResponse>>(
                        new PostApprenticeFeedbackRequest
                        {
                            Data = new PostApprenticeFeedbackRequestData { Ukprns = ukprns }
                        });

                foreach(var ratingResponse in apprenticeFeedbackRatings.Body)
                {
                    providers.Providers.First(s => s.Ukprn == ratingResponse.Ukprn).ApprenticeFeedback = ratingResponse;
                }
            }

            return new GetTrainingCourseProvidersResult
            {
                Course = courseTask.Result,
                Providers = providers.Providers,
                Total = providers.TotalResults,
                Location = locationTask.Result,
                ShortlistItemCount = shortlistTask.Result
            };
        }
    }
}