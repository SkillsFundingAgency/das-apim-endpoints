using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Application.TrainingCourses.Queries
{
    public class GetTrainingCoursesQueryHandler : IRequestHandler<GetTrainingCoursesQuery, GetTrainingCoursesQueryResult>
    {
        private readonly ICourseService _standardsService;

        public GetTrainingCoursesQueryHandler (ICourseService standardsService)
        {
            _standardsService = standardsService;
        }
        public async Task<GetTrainingCoursesQueryResult> Handle(GetTrainingCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _standardsService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            return new GetTrainingCoursesQueryResult
            {
                TrainingCourses = courses.Standards
            };
        }
    }
}