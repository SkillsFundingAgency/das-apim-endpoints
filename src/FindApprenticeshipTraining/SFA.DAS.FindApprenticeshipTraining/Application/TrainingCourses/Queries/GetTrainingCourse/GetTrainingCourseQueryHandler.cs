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

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;
        private readonly ILocationLookupService _locationLookupService;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseQueryHandler (
            ICoursesApiClient<CoursesApiConfiguration> apiClient,
            ICacheStorageService cacheStorageService,
            ILocationLookupService locationLookupService,
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient, 
            IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _apiClient = apiClient;
            _locationLookupService = locationLookupService;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _shortlistApiClient = shortlistApiClient;
            _cacheHelper = new CacheHelper(cacheStorageService);

        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var location = await _locationLookupService.GetLocationInformation(request.LocationName, request.Lat, request.Lon);
            
            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            var ukprnsCountTask = _roatpCourseManagementApiClient.Get<GetTotalProvidersForStandardResponse>(
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
                ProvidersCount = ukprnsCountTask.Result.ProvidersCount, 
                ProvidersCountAtLocation = ukprnsCountTask.Result.ProvidersCount,
                ShortlistItemCount = shortlistTask.Result
            };
        }
    }
}