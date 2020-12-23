using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListQueryHandler : IRequestHandler<GetTrainingCoursesListQuery, GetTrainingCoursesListResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCoursesListQueryHandler(ICoursesApiClient<CoursesApiConfiguration> apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheHelper = new CacheHelper(cacheStorageService);
        }

        public async Task<GetTrainingCoursesListResult> Handle(GetTrainingCoursesListQuery request, CancellationToken cancellationToken)
        {
            var taskList = new List<Task>();
        bool saveStandardsToCache;

        var sectors = await _cacheHelper.GetRequest<GetSectorsListResponse>(_apiClient,
                new GetSectorsListRequest(),nameof(GetSectorsListResponse), out var saveSectorsToCache);

            var routeFilters = sectors
                .Sectors
                .Where(c=>request.RouteIds.Contains(c.Route)).Select(x=>x.Id)
                .ToList();
            
            var standardsRequest = new GetAvailableToStartStandardsListRequest
            {
                Keyword = request.Keyword,
                RouteIds = routeFilters,
                Levels = request.Levels,
                OrderBy = (CoursesOrderBy)request.OrderBy
            };

            Task<GetStandardsListResponse> standardsTask;

            if (_cacheHelper.FilterApplied(standardsRequest))
            {
                standardsTask = _apiClient.Get<GetStandardsListResponse>(standardsRequest);
                saveStandardsToCache = false;
                
                taskList.Add(standardsTask);
            }
            else
            {
                standardsTask = _cacheHelper.GetRequest<GetStandardsListResponse>(_apiClient,
                    new GetAvailableToStartStandardsListRequest
                {
                    Keyword = request.Keyword,
                    RouteIds = routeFilters,
                    Levels = request.Levels,
                    OrderBy = (CoursesOrderBy)request.OrderBy
                }, nameof(GetStandardsListResponse), out saveStandardsToCache);
                taskList.Add(standardsTask);
            }

            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out var saveLevelsToCache);
            taskList.Add(levelsTask);
            
            await Task.WhenAll(taskList);

            await _cacheHelper.UpdateCachedItems(Task.FromResult(sectors), levelsTask, standardsTask, 
                new CacheHelper.SaveToCache{Levels = saveLevelsToCache, Sectors = saveSectorsToCache, Standards = saveStandardsToCache});
            
            return new GetTrainingCoursesListResult
            {
                Courses = standardsTask.Result.Standards,
                Sectors = sectors.Sectors,
                Levels = levelsTask.Result.Levels,
                Total = standardsTask.Result.Total,
                TotalFiltered = standardsTask.Result.TotalFiltered,
                OrderBy = request.OrderBy
            };
        }
    }
}
