using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider
{
    public class GetTrainingCourseProviderQueryHandler : IRequestHandler<GetTrainingCourseProviderQuery, GetTrainingCourseProviderResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly ICacheStorageService _cacheStorageService;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseProviderQueryHandler (
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient, ICacheStorageService cacheStorageService)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _coursesApiClient = coursesApiClient;
            _cacheStorageService = cacheStorageService;
            _cacheHelper = new CacheHelper(cacheStorageService);
        }
        public async Task<GetTrainingCourseProviderResult> Handle(GetTrainingCourseProviderQuery request, CancellationToken cancellationToken)
        {
            var providerTask = _courseDeliveryApiClient.Get<GetProviderStandardItem>(new GetProviderByCourseAndUkPrnRequest(request.ProviderId, request.CourseId));

            if (request.CourseIds.Count == 0)
            {
                var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.CourseId));
                await Task.WhenAll(courseTask, providerTask);

                return new GetTrainingCourseProviderResult
                {
                    Course = courseTask.Result,
                    ProviderStandard = providerTask.Result
                };
            }
            var saveToCache = false;
            var coursesTask = _cacheHelper.GetRequest<GetStandardsListResponse>(_coursesApiClient,
                new GetStandardsListRequest(), nameof(GetStandardsListResponse), out saveToCache);

            await Task.WhenAll(coursesTask, providerTask);

            return new GetTrainingCourseProviderResult
            {
                ProviderStandard = providerTask.Result,
                Courses = coursesTask.Result.Standards.ToList()
            };
            
        }
    }
}