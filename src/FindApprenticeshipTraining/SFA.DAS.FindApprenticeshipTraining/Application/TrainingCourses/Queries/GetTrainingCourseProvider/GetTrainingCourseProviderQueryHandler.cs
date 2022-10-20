using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderQueryHandler : IRequestHandler<GetTrainingCourseProviderQuery, GetTrainingCourseProviderResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IShortlistService _shortlistService;
        private readonly CacheHelper _cacheHelper;
        private readonly ILocationLookupService _locationLookupService;

        public GetTrainingCourseProviderQueryHandler(
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ICacheStorageService cacheStorageService,
            IShortlistService shortlistService, 
            ILocationLookupService locationLookupService)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _coursesApiClient = coursesApiClient;
            _shortlistService = shortlistService;
            _locationLookupService = locationLookupService;
            _cacheHelper = new CacheHelper(cacheStorageService);
        }
        public async Task<GetTrainingCourseProviderResult> Handle(GetTrainingCourseProviderQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.Location, request.Lat, request.Lon);
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));

            await Task.WhenAll(locationTask, courseTask);

            var ukprnsCount = _courseDeliveryApiClient.Get<GetUkprnsForStandardAndLocationResponse>(
                new GetUkprnsForStandardAndLocationRequest(request.CourseId, locationTask.Result?.GeoPoint?.FirstOrDefault() ?? 0,
                    locationTask.Result?.GeoPoint?.LastOrDefault() ?? 0));
            var providerTask = _courseDeliveryApiClient.Get<GetProviderStandardItem>(
                new GetProviderByCourseAndUkPrnRequest(request.ProviderId, request.CourseId, courseTask.Result.SectorSubjectAreaTier2Description,locationTask.Result?.GeoPoint?.FirstOrDefault(), locationTask.Result?.GeoPoint?.LastOrDefault(), request.ShortlistUserId));
            var providerCoursesTask = _courseDeliveryApiClient.Get<GetProviderAdditionalStandardsItem>(
                new GetProviderAdditionalStandardsRequest(request.ProviderId));
            var overallAchievementRatesTask = _courseDeliveryApiClient.Get<GetOverallAchievementRateResponse>(
                new GetOverallAchievementRateRequest(courseTask.Result.SectorSubjectAreaTier2Description));

            var apprenticeFeedbackTask = _apprenticeFeedbackApiClient.GetWithResponseCode<GetApprenticeFeedbackResponse>(new GetApprenticeFeedbackDetailsRequest(request.ProviderId));
            var employerFeedbackTask = _employerFeedbackApiClient.GetWithResponseCode<GetEmployerFeedbackResponse>(new GetEmployerFeedbackDetailsRequest(request.ProviderId));

            var coursesTask = _cacheHelper.GetRequest<GetStandardsListResponse>(_coursesApiClient,
                new GetAvailableToStartStandardsListRequest(), nameof(GetStandardsListResponse), out var saveToCache);

            var shortlistTask = _shortlistService.GetShortlistItemCount(request.ShortlistUserId);
            
            await Task.WhenAll(providerTask, coursesTask, providerCoursesTask, ukprnsCount, overallAchievementRatesTask, shortlistTask, apprenticeFeedbackTask, employerFeedbackTask);

            if (providerTask.Result == null && locationTask.Result != null)
            {
                providerTask = Task.FromResult(
                    await _courseDeliveryApiClient.Get<GetProviderStandardItem>(
                        new GetProviderByCourseAndUkPrnRequest(request.ProviderId, request.CourseId, courseTask.Result.SectorSubjectAreaTier2Description, null, null, request.ShortlistUserId)));

                if (providerTask.Result != null)
                {
                    providerTask.Result.DeliveryTypes = new List<GetDeliveryTypeItem> 
                    {
                        new GetDeliveryTypeItem
                        {
                            DeliveryModes = "NotFound"
                        } 
                    };    
                }
            }

            if(providerTask.Result != null && apprenticeFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && apprenticeFeedbackTask.Result.Body != null)
            {
                providerTask.Result.ApprenticeFeedback = apprenticeFeedbackTask.Result.Body;
            }
            
            if(providerTask.Result != null && employerFeedbackTask.Result?.StatusCode == System.Net.HttpStatusCode.OK && employerFeedbackTask.Result.Body != null)
            {
                providerTask.Result.EmployerFeedback = employerFeedbackTask.Result.Body;
            }

            await _cacheHelper.UpdateCachedItems(null, null, coursesTask,
                new CacheHelper.SaveToCache { Levels = false, Sectors = false, Standards = saveToCache });

            var additionalCourses = providerCoursesTask.Result.StandardIds.Any() 
                ? BuildAdditionalCoursesResponse(request, providerCoursesTask, coursesTask) 
                : new List<GetAdditionalCourseListItem>();

            return new GetTrainingCourseProviderResult
            {
                ProviderStandard = providerTask.Result,
                Course = courseTask.Result,
                AdditionalCourses = additionalCourses,
                OverallAchievementRates = overallAchievementRatesTask.Result.OverallAchievementRates,
                TotalProviders = ukprnsCount.Result.UkprnsByStandard.Count(),
                TotalProvidersAtLocation = ukprnsCount.Result.UkprnsByStandardAndLocation.Count(),
                Location = locationTask.Result,
                ShortlistItemCount = shortlistTask.Result
            };
        }

        private static IEnumerable<GetAdditionalCourseListItem> BuildAdditionalCoursesResponse(GetTrainingCourseProviderQuery request, Task<GetProviderAdditionalStandardsItem> providerCoursesTask, Task<GetStandardsListResponse> coursesTask)
        {
            return providerCoursesTask
                .Result
                .StandardIds.Select(courseId =>
                    coursesTask.Result.Standards.SingleOrDefault(c => c.LarsCode.Equals(courseId)))
                .Where(c => c != null)
                .Select(course => new GetAdditionalCourseListItem
                {
                    Id = course.LarsCode,
                    Level = course.Level,
                    Title = course.Title
                })
                .OrderBy(c => c.Title)
                .ToList();
        }
    }
}