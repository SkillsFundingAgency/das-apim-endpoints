using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Application.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Application.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList
{
    public class GetTrainingCoursesListQueryHandler : IRequestHandler<GetTrainingCoursesListQuery, GetTrainingCoursesListResult>
    {
        private readonly IApiClient _apiClient;

        public GetTrainingCoursesListQueryHandler(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetTrainingCoursesListResult> Handle(GetTrainingCoursesListQuery request, CancellationToken cancellationToken)
        {
            var standardsTask = _apiClient.Get<GetStandardsListResponse>(new GetStandardsListRequest
            {
                Keyword = request.Keyword, 
                RouteIds = request.RouteIds
            });
            var sectorsTask = _apiClient.Get<GetSectorsListResponse>(new GetSectorsListRequest());

            await Task.WhenAll(standardsTask, sectorsTask);

            return new GetTrainingCoursesListResult
            {
                Courses = standardsTask.Result.Standards,
                Sectors = sectorsTask.Result.Sectors,
                Total = standardsTask.Result.Total,
                TotalFiltered = standardsTask.Result.TotalFiltered
            };
        }
    }
}
