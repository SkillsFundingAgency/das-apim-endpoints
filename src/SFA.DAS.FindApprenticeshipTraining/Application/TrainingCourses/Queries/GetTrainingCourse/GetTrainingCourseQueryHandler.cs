using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private List<Task> _taskList;
        private bool _saveLevelsToCache;
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseQueryHandler (ICoursesApiClient<CoursesApiConfiguration> apiClient, ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _cacheHelper = new CacheHelper(cacheStorageService);

        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            _taskList = new List<Task>();

            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            var providersTask = _courseDeliveryApiClient.Get<GetUkprnsForStandardAndLocationResponse>(new GetUkprnsForStandardAndLocationRequest(request.Id, request.Lat, request.Lon));

            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _saveLevelsToCache);
            _taskList.Add(levelsTask);

            await Task.WhenAll(standardTask, providersTask, levelsTask);
            
            standardTask.Result.LevelEquivalent = levelsTask.Result.Levels.Where(x => x.Code == standardTask.Result.Level).FirstOrDefault().Name;

            return new GetTrainingCourseResult
            {
                Course = standardTask.Result,
                ProvidersCount = providersTask.Result.UkprnsByStandard.ToList().Count,
                ProvidersCountAtLocation = providersTask.Result.UkprnsByStandardAndLocation.ToList().Count,
            };
        }
    }
}