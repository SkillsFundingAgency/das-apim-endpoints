using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;
        private readonly IShortlistService _shortlistService;
        private readonly CacheHelper _cacheHelper;

        public GetTrainingCourseQueryHandler (
            ICoursesApiClient<CoursesApiConfiguration> apiClient, 
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient, 
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient,
            ICacheStorageService cacheStorageService,
            IShortlistService shortlistService)
        {
            _apiClient = apiClient;
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _employerDemandApiClient = employerDemandApiClient;
            _shortlistService = shortlistService;
            _cacheHelper = new CacheHelper(cacheStorageService);

        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            var providersTask = _courseDeliveryApiClient.Get<GetUkprnsForStandardAndLocationResponse>(new GetUkprnsForStandardAndLocationRequest(request.Id, request.Lat, request.Lon));

            var levelsTask = _cacheHelper.GetRequest<GetLevelsListResponse>(_apiClient,
                new GetLevelsListRequest(), nameof(GetLevelsListResponse), out _);

            var shortlistTask = _shortlistService.GetShortlistItemCount(request.ShortlistUserId);

            var showEmployerDemandTask = _employerDemandApiClient.Get<GetShowEmployerDemandResponse>(new GetShowEmployerDemandRequest());
            
            await Task.WhenAll(standardTask, providersTask, levelsTask, shortlistTask, showEmployerDemandTask);

            if (standardTask.Result == null)
            {
                return new GetTrainingCourseResult();
            }
            
            standardTask.Result.LevelEquivalent = levelsTask.Result.Levels.SingleOrDefault(x => x.Code == standardTask.Result.Level)?.Name;

            return new GetTrainingCourseResult
            {
                Course = standardTask.Result,
                ProvidersCount = providersTask.Result.UkprnsByStandard.ToList().Count,
                ProvidersCountAtLocation = providersTask.Result.UkprnsByStandardAndLocation.ToList().Count,
                ShortlistItemCount = shortlistTask.Result,
                ShowEmployerDemand = showEmployerDemandTask.Result != default
            };
        }
    }
}