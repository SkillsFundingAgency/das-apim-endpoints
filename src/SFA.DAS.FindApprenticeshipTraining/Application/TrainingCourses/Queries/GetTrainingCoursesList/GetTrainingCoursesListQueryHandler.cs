using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.CodeAnalysis;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListQueryHandler : IRequestHandler<GetTrainingCoursesListQuery, GetTrainingCoursesListResult>
    {
        private readonly IApiClient _apiClient;
        private readonly ICacheStorageService _cacheStorageService;
        private const int ExpirationInHours = 23;
        
        public GetTrainingCoursesListQueryHandler(IApiClient apiClient, ICacheStorageService cacheStorageService)
        {
            _apiClient = apiClient;
            _cacheStorageService = cacheStorageService;
        }

        public async Task<GetTrainingCoursesListResult> Handle(GetTrainingCoursesListQuery request, CancellationToken cancellationToken)
        {
            var taskList = new List<Task>();
            
            var standardsTask = _apiClient.Get<GetStandardsListResponse>(new GetStandardsListRequest
            {
                Keyword = request.Keyword, 
                RouteIds = request.RouteIds
            });
            taskList.Add(standardsTask);

            Task<GetSectorsListResponse> sectorsTask;
            var sectorsFromCache =
                await _cacheStorageService.RetrieveFromCache<GetSectorsListResponse>(nameof(GetSectorsListResponse));

            var saveSectorsToCache = false;
            if (sectorsFromCache != null)
            {
                sectorsTask = Task.FromResult(sectorsFromCache);
            }
            else
            {
                sectorsTask = _apiClient.Get<GetSectorsListResponse>(new GetSectorsListRequest());
                saveSectorsToCache = true;
            }
            
            taskList.Add(sectorsTask);
            
            await Task.WhenAll(taskList);

            if (saveSectorsToCache)
            {
                await _cacheStorageService
                    .SaveToCache(nameof(GetSectorsListResponse), sectorsTask.Result, ExpirationInHours);    
            }

            var filteredStandards = standardsTask
                .Result
                .Standards
                .Where(c=>
                    c.StandardDates.TrueForAll(
                        standardDate=>
                            (standardDate.LastDateStarts == null 
                            || standardDate.LastDateStarts >= DateTime.UtcNow)
                            && standardDate.LastDateStarts != standardDate.EffectiveFrom
                            && standardDate.EffectiveFrom <= DateTime.UtcNow
                            )).ToList();

            
            return new GetTrainingCoursesListResult
            {
                Courses = filteredStandards,
                Sectors = sectorsTask.Result.Sectors,
                Total = standardsTask.Result.Total,
                TotalFiltered = standardsTask.Result.TotalFiltered
            };
        }
    }
}
