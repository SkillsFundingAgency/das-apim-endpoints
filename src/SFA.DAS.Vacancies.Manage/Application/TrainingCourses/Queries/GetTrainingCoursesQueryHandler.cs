using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Application.TrainingCourses.Queries
{
    public class GetTrainingCoursesQueryHandler : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
    {
        private const int CourseCacheDurationInHours = 3;
        private readonly ICacheStorageService _cacheStorageService;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetTrainingCoursesQueryHandler (ICacheStorageService cacheStorageService, ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _cacheStorageService = cacheStorageService;
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses =
                await _cacheStorageService.RetrieveFromCache<GetStandardsListResponse>(
                    nameof(GetStandardsListResponse));

            if (courses != null)
            {
                return new GetTrainingCoursesQueryResult
                {
                    TrainingCourses = courses.Standards
                };
            }
            
            courses = await _coursesApiClient.Get<GetStandardsListResponse>(new GetActiveStandardsListRequest());
            await _cacheStorageService.SaveToCache(nameof(GetStandardsListResponse), courses, CourseCacheDurationInHours);

            return new GetTrainingCoursesQueryResult
            {
                TrainingCourses = courses.Standards
            };
        }
    }
}