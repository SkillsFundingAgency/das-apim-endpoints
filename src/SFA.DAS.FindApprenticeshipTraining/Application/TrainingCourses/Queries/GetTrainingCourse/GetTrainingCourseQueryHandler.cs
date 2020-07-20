using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;

        public GetTrainingCourseQueryHandler (ICoursesApiClient<CoursesApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetTrainingCourseResult> Handle(GetTrainingCourseQuery request, CancellationToken cancellationToken)
        {
            var standardTask = _apiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));
            
            await Task.WhenAll(standardTask);
            
            return new GetTrainingCourseResult
            {
                Course = standardTask.Result
            };
        }
    }
}