using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.TrainingCourses.Queries
{
    public class GetRoutesQueryHandler : IRequestHandler<GetRoutesQuery, GetRoutesQueryResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;

        public GetRoutesQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
        {
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
        }
        public async Task<GetRoutesQueryResult> Handle(GetRoutesQuery request, CancellationToken cancellationToken)
        {
            var response = await _cacheStorageService.RetrieveFromCache<GetRoutesListResponse>(nameof(GetRoutesListResponse));
            if (response == null)
            {
                response = await _coursesApiClient.Get<GetRoutesListResponse>(new GetRoutesListRequest());

                await _cacheStorageService.SaveToCache(nameof(GetRoutesListResponse), response, 23);
            }
            
            return new GetRoutesQueryResult
            {
                Routes = response.Routes
            };
        }
    }
}