using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.VacanciesManage.InnerApi.Responses;

namespace SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries
{
    public class GetTrainingCoursesQueryHandler : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
    {
        private readonly ICourseService _courseService;

        public GetTrainingCoursesQueryHandler (ICourseService courseService)
        {
            _courseService = courseService;
        }
        public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));
            
            return new GetTrainingCoursesQueryResult
            {
                TrainingCourses = courses.Standards
            };
        }
    }
}