using System.Collections.Generic;
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
        private readonly ICacheStorageService _cacheStorageService;
        private List<Task> _taskList;
        private bool _saveStandardsToCache;
        private bool _saveSectorsToCache;
        private bool _saveLevelsToCache;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCoursesListQueryHandler(ICoursesApiClient<CoursesApiConfiguration> apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheStorageService = cacheStorageService;
            _cacheHelper = new CacheHelper(cacheStorageService);
        }

        public async Task<GetTrainingCoursesListResult> Handle(GetTrainingCoursesListQuery request, CancellationToken cancellationToken)
        {
            _taskList = new List<Task>();
            var standardsRequest = new GetStandardsListRequest
            {
                Keyword = request.Keyword,
                RouteIds = request.RouteIds,
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
                    RouteIds = request.RouteIds,
                    Levels = request.Levels,
                    OrderBy = request.OrderBy
                }, nameof(GetStandardsListResponse), out _saveStandardsToCache);
                _taskList.Add(standardsTask);
            }

            var sectorsTask = _cacheHelper.GetRequest<GetSectorsListResponse>(_apiClient,
                new GetSectorsListRequest(),nameof(GetSectorsListResponse), out _saveSectorsToCache);
           _taskList.Add(sectorsTask);

            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _saveLevelsToCache);
            _taskList.Add(levelsTask);
            
            await Task.WhenAll(_taskList);

            await _cacheHelper.UpdateCachedItems(sectorsTask, levelsTask, standardsTask, 
                new SaveToCache{Levels = _saveLevelsToCache, Sectors = _saveSectorsToCache, Standards = _saveStandardsToCache});
            
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
        
    }
    internal class SaveToCache
    {
        public bool Sectors { get; set; }
        public bool Levels { get; set; }
        public bool Standards { get; set; }
    }
}
