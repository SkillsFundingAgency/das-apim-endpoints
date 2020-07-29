using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler : IRequestHandler<GetTrainingCourseProvidersQuery, GetTrainingCourseProvidersResult>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetTrainingCourseProvidersQueryHandler (
            ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingCourseProvidersResult> Handle(GetTrainingCourseProvidersQuery request, CancellationToken cancellationToken)
        {
            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            var providersTask = _courseDeliveryApiClient.Get<GetProvidersListResponse>(new GetProvidersByCourseRequest(request.Id));

            await Task.WhenAll(courseTask, providersTask);
            
            return new GetTrainingCourseProvidersResult
            {
                Course = courseTask.Result,
                Providers = providersTask.Result.Providers,
                Total = providersTask.Result.TotalResults
            }; 
        }
    }
}