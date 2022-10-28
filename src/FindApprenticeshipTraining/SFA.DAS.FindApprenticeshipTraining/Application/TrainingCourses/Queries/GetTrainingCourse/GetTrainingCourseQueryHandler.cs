using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpV2ApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private readonly IShortlistService _shortlistService;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> apiClient,
            ICacheStorageService cacheStorageService,
            IShortlistService shortlistService,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpV2ApiClient)
        {
            _apiClient = apiClient;
            _shortlistService = shortlistService;
            _cacheHelper = new CacheHelper(cacheStorageService);
            _roatpV2ApiClient = roatpV2ApiClient;
        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            var providersTask = _roatpV2ApiClient.Get<GetTotalProvidersForStandardResponse>(new GetTotalProvidersForStandardRequest(request.Id));

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
                ProvidersCount = providersTask.Result.ProvidersCount,
                ProvidersCountAtLocation = providersTask.Result.ProvidersCount,
                ShortlistItemCount = shortlistTask.Result
            };
        }
    }
}