using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Domain.Models;
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
            var locationTask = _locationHelper.GetLocationInformation(request.Location, request.Lat, request.Lon);
            
            var courseTask =  _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));

            await Task.WhenAll(locationTask, courseTask);
            
            var providers = await _courseDeliveryApiClient.Get<GetProvidersListResponse>(new GetProvidersByCourseRequest(request.Id, courseTask.Result.SectorSubjectAreaTier2Description,locationTask.Result?.GeoPoint?.FirstOrDefault(), locationTask.Result?.GeoPoint?.LastOrDefault(), request.SortOrder));
            
            return new GetTrainingCourseProvidersResult
            {
                Course = courseTask.Result,
                Providers = providers.Providers,
                Total = providers.TotalResults,
                Location = locationTask.Result
            }; 
        }
    }
}