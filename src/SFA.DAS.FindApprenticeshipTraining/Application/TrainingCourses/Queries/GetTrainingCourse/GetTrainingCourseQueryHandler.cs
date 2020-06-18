using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCourse
{
    public class GetTrainingCourseQueryHandler : IRequestHandler<GetTrainingCourseQuery,GetTrainingCourseResult>
    {
        private readonly IApiClient _apiClient;

        public GetTrainingCourseQueryHandler (IApiClient apiClient)
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