using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderQueryHandler : IRequestHandler<GetTrainingCourseProviderQuery, GetTrainingCourseProviderResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly CacheHelper _cacheHelper;
        private readonly LocationHelper _locationHelper;

        public GetTrainingCourseProviderQueryHandler(
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ILocationApiClient<LocationApiConfiguration> locationApiClient,
            ICacheStorageService cacheStorageService)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _coursesApiClient = coursesApiClient;
            _cacheHelper = new CacheHelper(cacheStorageService);
            _locationHelper = new LocationHelper(locationApiClient);
        }
        public async Task<GetTrainingCourseProviderResult> Handle(GetTrainingCourseProviderQuery request, CancellationToken cancellationToken)
        {
            var location = await _locationHelper.GetLocationInformation(request.Location);
            
            var providerTask = _courseDeliveryApiClient.Get<GetProviderStandardItem>(
                new GetProviderByCourseAndUkPrnRequest(request.ProviderId, request.CourseId, location?.Location.GeoPoint.First(), location?.Location.GeoPoint.Last()));
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
            var providerCoursesTask = _courseDeliveryApiClient.Get<GetProviderAdditionalStandardsItem>(
                    new GetProviderAdditionalStandardsRequest(request.ProviderId));

            var coursesTask = _cacheHelper.GetRequest<GetStandardsListResponse>(_coursesApiClient,
                new GetStandardsListRequest(), nameof(GetStandardsListResponse), out var saveToCache);

            await Task.WhenAll(courseTask, providerTask, coursesTask, providerCoursesTask);

            if (providerTask.Result == null && location != null)
            {
                providerTask = Task.FromResult(
                    await _courseDeliveryApiClient.Get<GetProviderStandardItem>(
                        new GetProviderByCourseAndUkPrnRequest(request.ProviderId, request.CourseId)));

                providerTask.Result.DeliveryTypes = new List<GetDeliveryTypeItem> { new GetDeliveryTypeItem() { DeliveryModes = "NoResult" } };
            }
            
            var overallAchievementRates =
                await _courseDeliveryApiClient.Get<GetOverallAchievementRateResponse>(
                    new GetOverallAchievementRateRequest(courseTask.Result.SectorSubjectAreaTier2Description));

            await _cacheHelper.UpdateCachedItems(null, null, coursesTask,
                new CacheHelper.SaveToCache { Levels = false, Sectors = false, Standards = saveToCache });

            if (!providerCoursesTask.Result.StandardIds.Any())
            {
                return new GetTrainingCourseProviderResult
                {
                    Course = courseTask.Result,
                    ProviderStandard = providerTask.Result,
                    AdditionalCourses = new List<GetAdditionalCourseListItem>(),
                    OverallAchievementRates = overallAchievementRates.OverallAchievementRates
                };
            }

            var additionalCourses = BuildAdditionalCoursesResponse(request, providerCoursesTask, coursesTask);

            return new GetTrainingCourseProviderResult
            {
                ProviderStandard = providerTask.Result,
                Course = courseTask.Result,
                AdditionalCourses = additionalCourses,
                OverallAchievementRates = overallAchievementRates.OverallAchievementRates,
            };
        }

        private static IEnumerable<GetAdditionalCourseListItem> BuildAdditionalCoursesResponse(GetTrainingCourseProviderQuery request, Task<GetProviderAdditionalStandardsItem> providerCoursesTask, Task<GetStandardsListResponse> coursesTask)
        {
            return providerCoursesTask
                .Result
                .StandardIds.Select(courseId =>
                    coursesTask.Result.Standards.SingleOrDefault(c => c.Id.Equals(courseId)))
                .Where(c => c != null)
                .Select(course => new GetAdditionalCourseListItem
                {
                    Id = course.Id,
                    Level = course.Level,
                    Title = course.Title
                })
                .Where(x => x.Id != request.CourseId)
                .OrderBy(c => c.Title)
                .ToList();
        }
    }
}