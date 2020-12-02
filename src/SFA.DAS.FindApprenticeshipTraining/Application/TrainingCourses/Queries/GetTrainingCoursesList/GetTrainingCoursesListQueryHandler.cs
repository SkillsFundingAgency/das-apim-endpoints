using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using SFA.DAS.FindApprenticeshipTraining.Infrastructure.Services;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListQueryHandler : IRequestHandler<GetTrainingCoursesListQuery, GetTrainingCoursesListResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private List<Task> _taskList;
        private bool _saveStandardsToCache;
        private bool _saveSectorsToCache;
        private bool _saveLevelsToCache;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCoursesListQueryHandler(ICoursesApiClient<CoursesApiConfiguration> apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheHelper = new CacheHelper(cacheStorageService);
        }

        public async Task<GetTrainingCoursesListResult> Handle(GetTrainingCoursesListQuery request, CancellationToken cancellationToken)
        {
            _taskList = new List<Task>();
            
            var sectors = await _cacheHelper.GetRequest<GetSectorsListResponse>(_apiClient,
                new GetSectorsListRequest(),nameof(GetSectorsListResponse), out _saveSectorsToCache);

            var routeFilters = sectors
                .Sectors
                .Where(c=>request.RouteIds.Contains(c.Route)).Select(x=>x.Id)
                .ToList();
            
            var standardsRequest = new GetStandardsListRequest
            {
                Keyword = request.Keyword,
                RouteIds = routeFilters,
                Levels = request.Levels,
                OrderBy = request.OrderBy
            };

            Task<GetStandardsListResponse> standardsTask;

            if (_cacheHelper.FilterApplied(standardsRequest))
            {
                standardsTask = _apiClient.Get<GetStandardsListResponse>(standardsRequest);
                _saveStandardsToCache = false;
                
                _taskList.Add(standardsTask);
            }
            else
            {
                standardsTask = _cacheHelper.GetRequest<GetStandardsListResponse>(_apiClient,
                    new GetStandardsListRequest
                {
                    Keyword = request.Keyword,
                    RouteIds = routeFilters,
                    Levels = request.Levels,
                    OrderBy = request.OrderBy
                }, nameof(GetStandardsListResponse), out _saveStandardsToCache);
                _taskList.Add(standardsTask);
            }

            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _saveLevelsToCache);
            _taskList.Add(levelsTask);
            
            await Task.WhenAll(_taskList);

            await _cacheHelper.UpdateCachedItems(Task.FromResult(sectors), levelsTask, standardsTask, 
                new CacheHelper.SaveToCache{Levels = _saveLevelsToCache, Sectors = _saveSectorsToCache, Standards = _saveStandardsToCache});
            
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
