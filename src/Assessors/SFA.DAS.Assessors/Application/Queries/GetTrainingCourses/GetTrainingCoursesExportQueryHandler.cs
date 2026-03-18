using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;


using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;

namespace SFA.DAS.Assessors.Application.Queries.GetTrainingCourses
{
    public class GetTrainingCoursesExportQueryHandler : IRequestHandler<GetTrainingCoursesExportQuery, GetTrainingCoursesExportResult>
    {
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;

        public GetTrainingCoursesExportQueryHandler (ICoursesApiClient<CoursesApiConfiguration> coursesApiClient)
        {
            _coursesApiClient = coursesApiClient;
        }
        public async Task<GetTrainingCoursesExportResult> Handle(GetTrainingCoursesExportQuery request, CancellationToken cancellationToken)
        {
            var standardsList = await _coursesApiClient.Get<GetStandardsExportListResponse>(new GetStandardsExportRequest());

            return new GetTrainingCoursesExportResult
            {
                TrainingCourses = standardsList.Standards
            };
        }
    }
}
