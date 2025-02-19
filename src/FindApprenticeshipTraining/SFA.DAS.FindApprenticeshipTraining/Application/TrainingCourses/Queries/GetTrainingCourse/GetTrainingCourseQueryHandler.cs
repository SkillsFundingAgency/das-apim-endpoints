using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseQueryHandler (
            ICoursesApiClient<CoursesApiConfiguration> apiClient,
            ICacheStorageService cacheStorageService,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient, 
            IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _apiClient = apiClient;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _shortlistApiClient = shortlistApiClient;
            _cacheHelper = new CacheHelper(cacheStorageService);

        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            var ukprnsCountTask = _roatpCourseManagementApiClient.Get<GetCourseTrainingProvidersCountResponse>(
                new GetCourseTrainingProvidersCountRequest(
                    [request.Id],
                    null,
                    null,
                    null
                )
            );

            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _);

            var shortlistTask =  request.ShortlistUserId.HasValue
                ? _shortlistApiClient.Get<int>(new GetShortlistUserItemCountRequest(request.ShortlistUserId.Value))
                : Task.FromResult(0);

            await Task.WhenAll(standardTask, ukprnsCountTask,  levelsTask, shortlistTask);

            var providersCountResponse = ukprnsCountTask?.Result?.Courses?.FirstOrDefault();

            if (standardTask.Result == null)
            {
                return new GetTrainingCourseResult();
            }
            
            standardTask.Result.LevelEquivalent = levelsTask.Result.Levels.SingleOrDefault(x => x.Code == standardTask.Result.Level)?.Name;

            return new GetTrainingCourseResult
            {
                Course = standardTask.Result,
                ProvidersCount = providersCountResponse?.ProvidersCount ?? 0,
                ProvidersCountAtLocation = providersCountResponse?.TotalProvidersCount ?? 0,
                ShortlistItemCount = shortlistTask.Result
            };
        }
    }
}