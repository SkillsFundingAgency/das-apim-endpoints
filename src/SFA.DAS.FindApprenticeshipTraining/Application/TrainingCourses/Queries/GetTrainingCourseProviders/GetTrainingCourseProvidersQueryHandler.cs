using System;
using System.Linq;
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
        private readonly ILocationApiClient<LocationApiConfiguration> _locationApiClient;

        public GetTrainingCourseProvidersQueryHandler (
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ILocationApiClient<LocationApiConfiguration> locationApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _coursesApiClient = coursesApiClient;
            _locationApiClient = locationApiClient;
        }
        public async Task<GetTrainingCourseProvidersResult> Handle(GetTrainingCourseProvidersQuery request, CancellationToken cancellationToken)
        {
            GetLocationsListItem location = null;
            if (!string.IsNullOrEmpty(request.Location) && request.Location.Split(",").Length == 2)
            {
                var locationInformation = request.Location.Split(",");
                var locationName = locationInformation.First().Trim();
                var authorityName = locationInformation.Last().Trim();
                location = await _locationApiClient.Get<GetLocationsListItem>(new GetLocationByLocationAndAuthorityName(locationName, authorityName));
            }    
            
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            var providersTask = _courseDeliveryApiClient.Get<GetProvidersListResponse>(new GetProvidersByCourseRequest(request.Id, location?.Location.GeoPoint.First(), location?.Location.GeoPoint.Last(), request.SortOrder));
            
            await Task.WhenAll(courseTask, providersTask);

            var providers = providersTask.Result;

            if (!string.IsNullOrEmpty(request.Filters) || request.Filters.Split(',').Length > 0)
            {
                var filters = request.Filters.Split(',');
                providers = providers.Providers.Where(c => c.DeliveryTypes.Any() == filters.Any()) as GetProvidersListResponse;
            }
            return new GetTrainingCourseProvidersResult
            {
                Course = courseTask.Result,
                Providers = providers.Providers,
                Total = providers.TotalResults
            }; 
        }
    }
}