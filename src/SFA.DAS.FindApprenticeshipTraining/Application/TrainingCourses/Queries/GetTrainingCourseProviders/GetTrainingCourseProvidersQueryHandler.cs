using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler : IRequestHandler<GetTrainingCourseProvidersQuery, GetTrainingCourseProvidersResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IEmployerDemandApiClient<EmployerDemandApiConfiguration> _employerDemandApiClient;
        private readonly IShortlistService _shortlistService;
        private readonly ILocationLookupService _locationLookupService;

        public GetTrainingCourseProvidersQueryHandler (
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            IEmployerDemandApiClient<EmployerDemandApiConfiguration> employerDemandApiClient,
            IShortlistService shortlistService, 
            ILocationLookupService locationLookupService)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _coursesApiClient = coursesApiClient;
            _employerDemandApiClient = employerDemandApiClient;
            _shortlistService = shortlistService;
            _locationLookupService = locationLookupService;
        }

        public async Task<GetTrainingCourseProvidersResult> Handle(GetTrainingCourseProvidersQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.Location, request.Lat, request.Lon);
            
            var courseTask =  _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));

            var shortlistTask = _shortlistService.GetShortlistItemCount(request.ShortlistUserId);

            var showEmployerDemandTask = _employerDemandApiClient.GetResponseCode(new GetShowEmployerDemandRequest());

            await Task.WhenAll(locationTask, courseTask, shortlistTask, showEmployerDemandTask);
            
            var providers = await _courseDeliveryApiClient.Get<GetProvidersListResponse>(new GetProvidersByCourseRequest(
                request.Id, 
                courseTask.Result.SectorSubjectAreaTier2Description, 
                courseTask.Result.Level,
                locationTask.Result?.GeoPoint?.FirstOrDefault(), 
                locationTask.Result?.GeoPoint?.LastOrDefault(), 
                request.SortOrder, 
                request.ShortlistUserId));
            
            return new GetTrainingCourseProvidersResult
            {
                Course = courseTask.Result,
                Providers = providers.Providers,
                Total = providers.TotalResults,
                Location = locationTask.Result,
                ShortlistItemCount = shortlistTask.Result,
                ShowEmployerDemand = showEmployerDemandTask.Result == HttpStatusCode.OK
            }; 
        }
    }
}