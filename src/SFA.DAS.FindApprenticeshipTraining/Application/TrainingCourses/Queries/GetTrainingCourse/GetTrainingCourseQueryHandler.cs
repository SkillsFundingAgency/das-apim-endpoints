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

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IShortlistService _shortlistService;
        private readonly ILocationLookupService _locationLookupService;
        private readonly FindApprenticeshipTrainingConfiguration _config;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseQueryHandler (
            ICoursesApiClient<CoursesApiConfiguration> apiClient, 
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            ICacheStorageService cacheStorageService,
            IShortlistService shortlistService,
            ILocationLookupService locationLookupService,
            IOptions<FindApprenticeshipTrainingConfiguration> config)
        {
            _apiClient = apiClient;
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _shortlistService = shortlistService;
            _locationLookupService = locationLookupService;
            _config = config.Value;
            _cacheHelper = new CacheHelper(cacheStorageService);

        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var location = await _locationLookupService.GetLocationInformation(request.LocationName, request.Lat, request.Lon);
            
            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            var providersTask = _courseDeliveryApiClient.Get<GetUkprnsForStandardAndLocationResponse>(new GetUkprnsForStandardAndLocationRequest(request.Id, location?.GeoPoint?.First() ?? 0, location?.GeoPoint?.Last() ?? 0));

            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _);

            var shortlistTask = _shortlistService.GetShortlistItemCount(request.ShortlistUserId);

            await Task.WhenAll(standardTask, providersTask, levelsTask, shortlistTask);

            if (standardTask.Result == null)
            {
                return new GetTrainingCourseResult();
            }
            
            standardTask.Result.LevelEquivalent = levelsTask.Result.Levels.SingleOrDefault(x => x.Code == standardTask.Result.Level)?.Name;

            return new GetTrainingCourseResult
            {
                Course = standardTask.Result,
                ProvidersCount = providersTask.Result.UkprnsByStandard.ToList().Count,
                ProvidersCountAtLocation = providersTask.Result.UkprnsByStandardAndLocation.ToList().Count,
                ShortlistItemCount = shortlistTask.Result
            };
        }
    }
}