using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
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
        private readonly ICacheStorageService _cacheStorageService;
        private const int ExpirationInHours = 1;
        private List<Task> _taskList;
        private bool _saveStandardsToCache;
        private bool _saveSectorsToCache;
        private bool _saveLevelsToCache;
        public GetTrainingCoursesListQueryHandler(ICoursesApiClient<CoursesApiConfiguration> apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<GetTrainingCoursesListResult> Handle(GetTrainingCoursesListQuery request, CancellationToken cancellationToken)
        {
            _taskList = new List<Task>();
            
            var standardsTask = GetRequest<GetStandardsListResponse>(new GetStandardsListRequest
            {
                Keyword = request.Keyword, 
                RouteIds = request.RouteIds,
                Levels = request.Levels,
                OrderBy = request.OrderBy
            }, nameof(GetStandardsListResponse), out _saveStandardsToCache);
            _taskList.Add(standardsTask);

            var sectorsTask = GetRequest<GetSectorsListResponse>(new GetSectorsListRequest(),nameof(GetSectorsListResponse), out _saveSectorsToCache);
           _taskList.Add(sectorsTask);

            var levelsTask = GetRequest<GetLevelsListResponse>(new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _saveLevelsToCache);
            _taskList.Add(levelsTask);
            
            await Task.WhenAll(_taskList);

            await UpdateCachedItems(sectorsTask, levelsTask, standardsTask);
            
            return new GetTrainingCoursesListResult
            {
                Courses = standardsTask.Result.Standards,
                Sectors = sectorsTask.Result.Sectors,
                Levels = levelsTask.Result.Levels,
                Total = standardsTask.Result.Total,
                TotalFiltered = standardsTask.Result.TotalFiltered,
                OrderBy = request.OrderBy
            };
        }

        private async Task UpdateCachedItems(Task<GetSectorsListResponse> sectorsTask,
            Task<GetLevelsListResponse> levelsTask,
            Task<GetStandardsListResponse> standardsTask)
        {
            if (_saveSectorsToCache)
            {
                await _cacheStorageService
                    .SaveToCache(nameof(GetSectorsListResponse), sectorsTask.Result, ExpirationInHours);
            }

            if (_saveLevelsToCache)
            {
                await _cacheStorageService.SaveToCache(nameof(GetLevelsListResponse), levelsTask.Result,
                    ExpirationInHours);
            }

            if (_saveStandardsToCache)
            {
                await _cacheStorageService
                    .SaveToCache(nameof(GetStandardsListResponse), standardsTask.Result, ExpirationInHours);
            }
        }

        private Task<TResponse> GetRequest<TResponse>(IGetApiRequest request,string keyName, out bool updateCache)
        {
            Task<TResponse> levelsTask;
            updateCache = false;
            
            var itemFromCache =
                _cacheStorageService.RetrieveFromCache<TResponse>(keyName).Result;

            if (itemFromCache != null)
            {
                levelsTask = Task.FromResult(itemFromCache);
            }
            else
            {
                levelsTask = _apiClient.Get<TResponse>(request);
                updateCache = true;
            }

            return levelsTask;
        }
        
    }
}
