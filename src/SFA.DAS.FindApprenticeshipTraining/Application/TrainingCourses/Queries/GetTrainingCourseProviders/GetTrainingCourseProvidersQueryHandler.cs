using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler : IRequestHandler<GetTrainingCourseProvidersQuery, GetTrainingCourseProvidersResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly LocationHelper _locationHelper;

        public GetTrainingCourseProvidersQueryHandler (
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ILocationApiClient<LocationApiConfiguration> locationApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _coursesApiClient = coursesApiClient;
            _locationHelper = new LocationHelper(locationApiClient);
        }
        public async Task<GetTrainingCourseProvidersResult> Handle(GetTrainingCourseProvidersQuery request, CancellationToken cancellationToken)
        {
            var location = await _locationHelper.GetLocationInformation(request.Location);
            var locationResult = !string.IsNullOrEmpty(location?.Postcode) ?
                                    location.Postcode : !(string.IsNullOrEmpty(location?.LocationName) && string.IsNullOrEmpty(location?.LocalAuthorityName)) ?
                                        $"{location.LocationName}, {location.LocalAuthorityName}" : null;

            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            var providersTask = _courseDeliveryApiClient.Get<GetProvidersListResponse>(new GetProvidersByCourseRequest(request.Id, location?.Location?.GeoPoint.First(), location?.Location?.GeoPoint.Last(), request.SortOrder));

            await Task.WhenAll(courseTask, providersTask);
            
            return new GetTrainingCourseProvidersResult
            {
                Course = courseTask.Result,
                Providers = providersTask.Result.Providers,
                Total = providersTask.Result.TotalResults,
                Location = locationResult,
            }; 
        }
    }
}