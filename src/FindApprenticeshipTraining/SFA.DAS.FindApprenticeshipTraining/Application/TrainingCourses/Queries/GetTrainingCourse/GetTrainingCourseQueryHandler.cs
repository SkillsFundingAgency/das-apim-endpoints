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
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpV2ApiClient;
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseQueryHandler(
            ICoursesApiClient<CoursesApiConfiguration> apiClient,
            ICacheStorageService cacheStorageService,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpV2ApiClient)
        //    IOptions<FindApprenticeshipTrainingConfiguration> config, 
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpV2ApiClient, 
            IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _apiClient = apiClient;
            _roatpV2ApiClient = roatpV2ApiClient;
            _shortlistApiClient = shortlistApiClient;
        //    _config = config.Value;
            _cacheHelper = new CacheHelper(cacheStorageService);
            _roatpV2ApiClient = roatpV2ApiClient;
        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            var ukprnsCountTask = _roatpV2ApiClient.Get<GetTotalProvidersForStandardResponse>(
                new GetTotalProvidersForStandardRequest(request.Id));


            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _);

            var shortlistTask =  request.ShortlistUserId.HasValue
                ? _shortlistApiClient.Get<int>(new GetShortlistUserItemCountRequest(request.ShortlistUserId.Value))
                : Task.FromResult(0);

            await Task.WhenAll(standardTask, ukprnsCountTask,  levelsTask, shortlistTask);

            if (standardTask.Result == null)
            {
                return new GetTrainingCourseResult();
            }
            
            standardTask.Result.LevelEquivalent = levelsTask.Result.Levels.SingleOrDefault(x => x.Code == standardTask.Result.Level)?.Name;

            return new GetTrainingCourseResult
            {
                Course = standardTask.Result,
                ProvidersCount = 0,
                ProvidersCountAtLocation = ukprnsCountTask.Result.ProvidersCount,
                ShortlistItemCount = shortlistTask.Result
            };
        }
    }
}